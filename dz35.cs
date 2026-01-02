using System;
using System.Collections.Generic;

//character
abstract class Character
{
    public string Name { get; private set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; private set; }

    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public List<Effect> ActiveEffects { get; private set; }

    protected Character(string name, int maxHealth)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        ActiveEffects = new List<Effect>();
    }

    public virtual void TakeDamage(int amount)
    {
        int finalDamage = amount;

        foreach (Effect effect in ActiveEffects)
        {
            finalDamage = effect.ModifyIncomingDamage(finalDamage);
        }

        Health -= finalDamage;
        if (Health < 0)
            Health = 0;

        Console.WriteLine(Name + " took " + finalDamage +
                          " damage. HP: " + Health + "/" + MaxHealth);
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth)
            Health = MaxHealth;

        Console.WriteLine(Name + " healed " + amount +
                          " HP. HP: " + Health + "/" + MaxHealth);
    }

    public void AddEffect(Effect effect)
    {
        ActiveEffects.Add(effect);
        effect.OnApply(this);
    }

    public void ProcessEffects()
    {
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffects[i].OnTurn(this);
            ActiveEffects[i].Duration--;

            if (ActiveEffects[i].Duration <= 0)
            {
                ActiveEffects[i].OnExpire(this);
                ActiveEffects.RemoveAt(i);
            }
        }
    }
}

// mage
class Mage : Character
{
    public List<Spell> Spells { get; private set; }

    public Mage(string name)
        : base(name, 100)
    {
        Spells = new List<Spell>();
    }

    public void CastSpell(int index, Character target)
    {
        if (index < 0 || index >= Spells.Count)
            return;

        Spell spell = Spells[index];

        if (!spell.CanCast())
        {
            Console.WriteLine(spell.Name + " is on cooldown");
            return;
        }

        spell.Cast(this, target);
    }
}

// goblin
class Goblin : Character
{
    public Goblin(string name)
        : base(name, 125)
    {
    }

    public override void TakeDamage(int amount)
    {
        int reduced = amount - 1;
        if (reduced < 0)
            reduced = 0;

        base.TakeDamage(reduced);
    }
}

// spell
abstract class Spell
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public int Cooldown { get; private set; }
    public int CurrentCooldown { get; private set; }

    protected Spell(string name, string description, int cooldown)
    {
        Name = name;
        Description = description;
        Cooldown = cooldown;
        CurrentCooldown = 0;
    }

    public bool CanCast()
    {
        return CurrentCooldown == 0;
    }

    public void ReduceCooldown()
    {
        if (CurrentCooldown > 0)
            CurrentCooldown--;
    }

    public void Cast(Character caster, Character target)
    {
        Apply(caster, target);
        CurrentCooldown = Cooldown;
    }

    protected abstract void Apply(Character caster, Character target);
}

// spells
class Fireball : Spell
{
    private Random random;

    public Fireball()
        : base("Fireball", "Deals damage and may apply burning", 2)
    {
        random = new Random();
    }

    protected override void Apply(Character caster, Character target)
    {
        Console.WriteLine(caster.Name + " casts Fireball!");
        target.TakeDamage(20);

        if (random.Next(100) < 40)
        {
            target.AddEffect(new Burning());
        }
    }
}

class HealSpell : Spell
{
    public HealSpell()
        : base("Heal", "Restores health", 2)
    {
    }

    protected override void Apply(Character caster, Character target)
    {
        Console.WriteLine(caster.Name + " casts Heal!");
        target.Heal(25);
    }
}

class ShieldSpell : Spell
{
    public ShieldSpell()
        : base("Shield", "Grants temporary protection", 3)
    {
    }

    protected override void Apply(Character caster, Character target)
    {
        Console.WriteLine(caster.Name + " casts Shield!");
        target.AddEffect(new Shielded());
    }
}

// effect
abstract class Effect
{
    public string Name { get; private set; }
    public int Duration { get; set; }

    protected Effect(string name, int duration)
    {
        Name = name;
        Duration = duration;
    }

    public virtual void OnApply(Character target)
    {
        Console.WriteLine(target.Name + " gained effect: " + Name);
    }

    public virtual void OnTurn(Character target)
    {
    }

    public virtual void OnExpire(Character target)
    {
        Console.WriteLine("Effect " + Name + " expired on " + target.Name);
    }

    public virtual int ModifyIncomingDamage(int damage)
    {
        return damage;
    }
}

// effects
class Burning : Effect
{
    public Burning()
        : base("Burning", 3)
    {
    }

    public override void OnTurn(Character target)
    {
        Console.WriteLine(target.Name + " is burning!");
        target.TakeDamage(5);
    }
}

class Shielded : Effect
{
    public Shielded()
        : base("Shielded", 5)
    {
    }

    public override int ModifyIncomingDamage(int damage)
    {
        int reduced = damage - 5;
        if (reduced < 0)
            reduced = 0;

        return reduced;
    }
}

// program
class Program
{
    static void Main(string[] args)
    {
        Mage mage = new Mage("Mage");
        Goblin goblin = new Goblin("Goblin");

        mage.Spells.Add(new Fireball());
        mage.Spells.Add(new HealSpell());
        mage.Spells.Add(new ShieldSpell());

        int round = 1;

        while (mage.IsAlive && goblin.IsAlive)
        {
            Console.WriteLine("\n ROUND " + round + " ");

            mage.CastSpell(0, goblin);

            mage.ProcessEffects();
            goblin.ProcessEffects();

            if (!goblin.IsAlive)
                break;

            Console.WriteLine("Goblin attacks");
            mage.TakeDamage(15);

            mage.ProcessEffects();
            goblin.ProcessEffects();

            foreach (Spell spell in mage.Spells)
            {
                spell.ReduceCooldown();
            }

            round++;
        }

        Console.WriteLine("\nBattle Ended!!!!");
        Console.WriteLine(mage.IsAlive ? "Mage wins!" : "Goblin wins!");
    }
}

