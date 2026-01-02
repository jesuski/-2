using System;
using System.Collections.Generic;

// interfaces
interface IUsable
{
    void Use(Character user);
}

interface IEquipable
{
    void Equip(Character user);
}

interface ISellable
{
    int Price { get; }
    void Sell(Character user);
}

interface IDiscardable
{
    void Discard();
}

interface IStackable
{
    int Count { get; }
    void AddOne();
    void RemoveOne();
}

// character
class Character
{
    public string Name { get; private set; }

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    public int Energy { get; private set; }
    public int MaxEnergy { get; private set; }

    public int Gold { get; private set; }

    public Weapon EquippedWeapon { get; private set; }
    public Armor EquippedArmor { get; private set; }

    public Character(string name)
    {
        Name = name;
        MaxHealth = 130;
        Health = 130;
        MaxEnergy = 70;
        Energy = 70;
        Gold = 0;
    }

    public void AddHealth(int value)
    {
        Health += value;
        if (Health < 0) Health = 0;
        if (Health > MaxHealth) Health = MaxHealth;
    }

    public void AddEnergy(int value)
    {
        Energy += value;
        if (Energy < 0) Energy = 0;
        if (Energy > MaxEnergy) Energy = MaxEnergy;
    }

    public void AddGold(int value)
    {
        Gold += value;
        if (Gold < 0) Gold = 0;
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
    }

    public void EquipArmor(Armor armor)
    {
        EquippedArmor = armor;
    }

    public void UnequipWeapon()
    {
        EquippedWeapon = null;
    }

    public void UnequipArmor()
    {
        EquippedArmor = null;
    }

    public void ShowInfo()
    {
        Console.WriteLine("Character Info");
        Console.WriteLine("Name: " + Name);
        Console.WriteLine("Health: " + Health + "/" + MaxHealth);
        Console.WriteLine("Energy: " + Energy + "/" + MaxEnergy);
        Console.WriteLine("Gold: " + Gold);
        Console.WriteLine("Weapon: " + (EquippedWeapon != null ? EquippedWeapon.Name : "None"));
        Console.WriteLine("Armor: " + (EquippedArmor != null ? EquippedArmor.Name : "None"));
        Console.WriteLine();
    }
}

// item
class Item
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public virtual void ShowInfo()
    {
        Console.WriteLine(Name + " - " + Description);
    }
}

// items
class HealthPotion : Item, IUsable, IStackable
{
    public int Count { get; private set; }

    public HealthPotion()
        : base("Health Potion", "Restores health")
    {
        Count = 1;
    }

    public void Use(Character user)
    {
        user.AddHealth(20);
        Console.WriteLine("Health restored.");
        RemoveOne();
    }

    public void AddOne()
    {
        Count++;
    }

    public void RemoveOne()
    {
        Count--;
    }
}

class Food : Item, IUsable, IStackable
{
    public int Count { get; private set; }

    public Food()
        : base("Food", "Restores energy")
    {
        Count = 1;
    }

    public void Use(Character user)
    {
        user.AddEnergy(15);
        Console.WriteLine("Energy restored.");
        RemoveOne();
    }

    public void AddOne()
    {
        Count++;
    }

    public void RemoveOne()
    {
        Count--;
    }
}

class Gem : Item, ISellable
{
    public int Price { get { return 50; } }

    public Gem()
        : base("Gem", "Valuable gemstone")
    {
    }

    public void Sell(Character user)
    {
        user.AddGold(Price);
        Console.WriteLine("Gem sold for " + Price + " gold.");
    }
}

class OldBoot : Item, IDiscardable
{
    public OldBoot()
        : base("Old Boot", "Useless old boot")
    {
    }

    public void Discard()
    {
        Console.WriteLine("The old boot was thrown away.");
    }
}

class Weapon : Item, IEquipable
{
    public Weapon(string name, string description)
        : base(name, description)
    {
    }

    public void Equip(Character user)
    {
        user.EquipWeapon(this);
        Console.WriteLine("Weapon equipped");
    }
}

class Armor : Item, IEquipable
{
    public Armor(string name, string description)
        : base(name, description)
    {
    }

    public void Equip(Character user)
    {
        user.EquipArmor(this);
        Console.WriteLine("Armor equipped");
    }
}

class MagicRing : Item, IEquipable, ISellable
{
    public int Price { get { return 100; } }

    public MagicRing()
        : base("Magic Ring", "Mysterious ring")
    {
    }

    public void Equip(Character user)
    {
        Console.WriteLine("Ring equipped.");
    }

    public void Sell(Character user)
    {
        user.AddGold(Price);
        Console.WriteLine("Ring sold.");
    }
}

class Scroll : Item, IUsable, ISellable
{
    public int Price { get { return 30; } }

    public Scroll()
        : base("Scroll", "Magic scroll")
    {
    }

    public void Use(Character user)
    {
        Console.WriteLine("Scroll used.");
    }

    public void Sell(Character user)
    {
        user.AddGold(Price);
        Console.WriteLine("Scroll sold.");
    }
}

// inventory
class Inventory
{
    private List<Item> items;
    private int maxSize;

    public Inventory(int maxSize)
    {
        this.maxSize = maxSize;
        items = new List<Item>();
    }

    public void Add(Item item)
    {
        if (item is IStackable)
        {
            foreach (Item existing in items)
            {
                if (existing.GetType() == item.GetType() & existing is IStackable)
                {
                    ((IStackable)existing).AddOne();
                    return;
                }
            }
        }

        if (items.Count < maxSize)
            items.Add(item);
    }

    public Item GetItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return null;
        return items[index];
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= items.Count)
            return;

        items.RemoveAt(index);
    }

    public void ShowInventory()
    {
        Console.WriteLine("Inventory");

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];

            if (item is IStackable)
            {
                Console.WriteLine("[" + i + "] " + item.Name +
                    " x" + ((IStackable)item).Count + " - " + item.Description);
            }
            else
            {
                Console.WriteLine("[" + i + "] " + item.Name + " - " + item.Description);
            }
        }

        Console.WriteLine();
    }
}

// program
class Program
{
    static void Main(string[] args)
    {
        Character character = new Character("Hero");
        Inventory inventory = new Inventory(15);

        inventory.Add(new HealthPotion());
        inventory.Add(new HealthPotion());
        inventory.Add(new Food());
        inventory.Add(new Gem());
        inventory.Add(new OldBoot());
        inventory.Add(new Weapon("Sword", "Old rusty sword"));
        inventory.Add(new Armor("Armor", "Leather armor"));

        while (true)
        {
            character.ShowInfo();
            inventory.ShowInventory();

            Console.WriteLine("Choose action:");
            Console.WriteLine("1 - Use item");
            Console.WriteLine("2 - Equip item");
            Console.WriteLine("3 - Sell item");
            Console.WriteLine("4 - Discard item");
            Console.WriteLine("0 - Exit");

            int action;
            if (!int.TryParse(Console.ReadLine(), out action))
                continue;

            if (action == 0)
                break;

            Console.Write("Enter item index: ");
            int index;
            if (!int.TryParse(Console.ReadLine(), out index))
                continue;

            Item item = inventory.GetItem(index);
            if (item == null)
                continue;

            if (action == 1)
            {
                if (item is IUsable)
                {
                    ((IUsable)item).Use(character);

                    if (item is IStackable && ((IStackable)item).Count <= 0)
                        inventory.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("This item cannot be used.");
                }
            }
            else if (action == 2)
            {
                if (item is IEquipable)
                {
                    ((IEquipable)item).Equip(character);
                }
                else
                {
                    Console.WriteLine("This item cannot be equipped.");
                }
            }
            else if (action == 3)
            {
                if (item is ISellable)
                {
                    ((ISellable)item).Sell(character);
                    inventory.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("This item cannot be sold.");
                }
            }
            else if (action == 4)
            {
                if (item is IDiscardable)
                {
                    ((IDiscardable)item).Discard();
                    inventory.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("This item cannot be discarded.");
                }
            }
        }
    }
}

