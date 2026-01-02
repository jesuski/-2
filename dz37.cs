using System;
using System.Collections.Generic;

// enums
enum CharacterClass
{
    Mage,
    Warrior
}

enum ArmorSlot
{
    Head,
    Body,
    Hands,
    Legs,
    Boots,
    Ring,
    Necklace
}

// character
class Character
{
    public string Name { get; private set; }
    public CharacterClass Class { get; private set; }

    public int Damage { get; set; }
    public int Defense { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }

    public EquipmentManager Equipment { get; private set; }

    public Character(string name, CharacterClass characterClass)
    {
        Name = name;
        Class = characterClass;

        Damage = 5;
        Defense = 0;
        Strength = 0;
        Intelligence = 0;

        Equipment = new EquipmentManager(this);
    }

    public void ShowStats()
    {
        Console.WriteLine("character stats");
        Console.WriteLine("Name: " + Name);
        Console.WriteLine("Class: " + Class);
        Console.WriteLine("Damage: " + Damage);
        Console.WriteLine("Defense: " + Defense);
        Console.WriteLine("Strength: " + Strength);
        Console.WriteLine("Intelligence: " + Intelligence);
        Console.WriteLine();
    }
}

// item
class Item
{
    public string Name { get; protected set; }

    public Item(string name)
    {
        Name = name;
    }
}

// weapon
class Weapon : Item
{
    public int BaseDamage { get; private set; }
    public int BonusStrength { get; private set; }
    public int BonusIntelligence { get; private set; }

    public Weapon(string name, int baseDamage, int bonusStr, int bonusInt)
        : base(name)
    {
        BaseDamage = baseDamage;
        BonusStrength = bonusStr;
        BonusIntelligence = bonusInt;
    }
}

//  armor
class Armor : Item
{
    public ArmorSlot Slot { get; private set; }
    public int Defense { get; private set; }

    public int BonusStrength { get; private set; }
    public int BonusIntelligence { get; private set; }

    public CharacterClass AllowedClass { get; private set; }

    public Armor(
        string name,
        ArmorSlot slot,
        int defense,
        int bonusStr,
        int bonusInt,
        CharacterClass allowedClass)
        : base(name)
    {
        Slot = slot;
        Defense = defense;
        BonusStrength = bonusStr;
        BonusIntelligence = bonusInt;
        AllowedClass = allowedClass;
    }
}

// equıpment manager
class EquipmentManager
{
    private Character owner;

    private Weapon weapon;

    private Dictionary<ArmorSlot, Armor> armorSlots;
    private Armor ring1;
    private Armor ring2;
    private Armor necklace;

    public EquipmentManager(Character owner)
    {
        this.owner = owner;
        armorSlots = new Dictionary<ArmorSlot, Armor>();
    }

    // weapon
    public bool EquipWeapon(Weapon newWeapon)
    {
        if (weapon != null)
        {
            Console.WriteLine("Weapon slot is already occupied.");
            return false;
        }

        weapon = newWeapon;
        owner.Damage += newWeapon.BaseDamage;
        owner.Strength += newWeapon.BonusStrength;
        owner.Intelligence += newWeapon.BonusIntelligence;

        return true;
    }

    public void UnequipWeapon()
    {
        if (weapon == null)
            return;

        owner.Damage -= weapon.BaseDamage;
        owner.Strength -= weapon.BonusStrength;
        owner.Intelligence -= weapon.BonusIntelligence;

        weapon = null;
    }

    // armor
    public bool EquipArmor(Armor armor)
    {
        if (armor.AllowedClass != owner.Class)
        {
            Console.WriteLine("This armor cannot be equipped by this class.");
            return false;
        }

        if (armor.Slot == ArmorSlot.Ring)
        {
            return EquipRing(armor);
        }

        if (armor.Slot == ArmorSlot.Necklace)
        {
            return EquipNecklace(armor);
        }

        if (armorSlots.ContainsKey(armor.Slot))
        {
            Console.WriteLine("Armor slot is already occupied.");
            return false;
        }

        armorSlots.Add(armor.Slot, armor);
        ApplyArmorBonuses(armor);
        return true;
    }

    public void UnequipArmor(ArmorSlot slot)
    {
        if (!armorSlots.ContainsKey(slot))
            return;

        RemoveArmorBonuses(armorSlots[slot]);
        armorSlots.Remove(slot);
    }

    // rings
    private bool EquipRing(Armor ring)
    {
        if (ring1 == null)
        {
            ring1 = ring;
            ApplyArmorBonuses(ring);
            return true;
        }

        if (ring2 == null)
        {
            ring2 = ring;
            ApplyArmorBonuses(ring);
            return true;
        }

        Console.WriteLine("Both ring slots are occupied.");
        return false;
    }

    public void UnequipRing(int index)
    {
        if (index == 1 && ring1 != null)
        {
            RemoveArmorBonuses(ring1);
            ring1 = null;
        }
        else if (index == 2 && ring2 != null)
        {
            RemoveArmorBonuses(ring2);
            ring2 = null;
        }
    }

    // necklace
    private bool EquipNecklace(Armor newNecklace)
    {
        if (necklace != null)
        {
            Console.WriteLine("Necklace slot is occupied");
            return false;
        }

        necklace = newNecklace;
        ApplyArmorBonuses(newNecklace);
        return true;
    }

    public void UnequipNecklace()
    {
        if (necklace == null)
            return;

        RemoveArmorBonuses(necklace);
        necklace = null;
    }

    // bonus hanndling
    private void ApplyArmorBonuses(Armor armor)
    {
        owner.Defense += armor.Defense;
        owner.Strength += armor.BonusStrength;
        owner.Intelligence += armor.BonusIntelligence;
    }

    private void RemoveArmorBonuses(Armor armor)
    {
        owner.Defense -= armor.Defense;
        owner.Strength -= armor.BonusStrength;
        owner.Intelligence -= armor.BonusIntelligence;
    }
}

// program
class Program
{
    static void Main(string[] args)
    {
        Character hero = new Character("Hero", CharacterClass.Warrior);

        Weapon sword = new Weapon("Sword", 10, 2, 0);

        Armor helmet = new Armor("Helmet", ArmorSlot.Head, 2, 0, 0, CharacterClass.Warrior);
        Armor ring = new Armor("Ring of Power", ArmorSlot.Ring, 0, 1, 0, CharacterClass.Warrior);
        Armor necklace = new Armor("Necklace of Mind", ArmorSlot.Necklace, 0, 0, 2, CharacterClass.Warrior);

        hero.Equipment.EquipWeapon(sword);
        hero.Equipment.EquipArmor(helmet);
        hero.Equipment.EquipArmor(ring);
        hero.Equipment.EquipArmor(necklace);

        hero.ShowStats();

        hero.Equipment.UnequipWeapon();
        hero.Equipment.UnequipRing(1);

        hero.ShowStats();
    }
}
