using System;

namespace GameEngineFirstLevel
{
    
    abstract class GameObject
    {
        private static int nextId = 1;

        public int Id { get; }
        public string Name { get; set; }
        public bool IsActive { get; private set; } = true;

        public GameObject(string name)
        {
            Id = nextId;
            nextId++;
            Name = name;
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }

        public abstract string Info();
    }

    
    interface IInteractable
    {
        string Interact(Player player);
    }

    interface ITriggerable
    {
        void Trigger();
    }

    interface IDamageable
    {
        void ApplyDamage(int amount);
    }

   
    class Player
    {
        public int Hp { get; set; }
        public bool HasAccessCard { get; set; }
        public int LastCheckpointId { get; set; }

        public Player()
        {
            Hp = 300;
            HasAccessCard = false;
            LastCheckpointId = 0;
        }
    }

  
    class Door : GameObject, IInteractable
    {
        private const string V = "No access";
        private bool requiredAccess;  // private field as required

        public Door(string name, bool requiresAccess) : base(name)
        {
            requiredAccess = requiresAccess;
        }

        public string Interact(Player player)
        {
            if (!IsActive)
                return "Door is not active";

            if (!requiredAccess || player.HasAccessCard)
            {
                return "Door opened";  // exactly as in requirements
            }
            else
            {
                return V;  // exactly as in requirements
            }
        }

        public override string Info()
        {
            return $"Door: {Name} (ID:{Id}), Active:{IsActive}, NeedsAccess:{requiredAccess}";
        }
    }

    
    class Checkpoint : GameObject, IInteractable
    {
        public Checkpoint(string name) : base(name) { }

        public string Interact(Player player)
        {
            player.LastCheckpointId = Id;  
            return $"Checkpoint {Id} saved";
        }

        public override string Info()
        {
            return $"Checkpoint: {Name} (ID:{Id}), Active:{IsActive}";
        }
    }

    
    class Trap : GameObject, IInteractable, IDamageable
    {
        public int Damage { get; }
        private bool isBroken = false;

        public Trap(string name, int damage) : base(name)
        {
            Damage = damage;
        }

        public string Interact(Player player)
        {
            if (!IsActive || isBroken)
                return "Trap is not active";

            player.Hp -= Damage;  
            return $"Trap dealt {Damage} damage. Player HP: {player.Hp}";
        }

        public void ApplyDamage(int amount)
        {
            if (amount > 0)  // simplified condition to break
            {
                isBroken = true;
                Disable();  
            }
        }

        public override string Info()
        {
            return $"Trap: {Name} (ID:{Id}), Active:{IsActive}, Damage:{Damage}, Broken:{isBroken}";
        }
    }

    
    class Program
    {
        static void Main()
        {
            

            Player player = new Player();

            
            Door door1 = new Door("Main Door", true);
            Checkpoint checkpoint1 = new Checkpoint("Start");
            Trap trap1 = new Trap("booby trap", 150);

            Console.WriteLine("1. Door without access card:");
            player.HasAccessCard = false;
            Console.WriteLine(door1.Interact(player));  

            Console.WriteLine("\n2.  Door with access card:");
            player.HasAccessCard = true;
            Console.WriteLine(door1.Interact(player)); 

            Console.WriteLine("\n3. Checkpoint:");
            Console.WriteLine(checkpoint1.Interact(player));
            Console.WriteLine($"LastCheckpointId = {player.LastCheckpointId}");

            Console.WriteLine("\n4. Trap interaction:");
            Console.WriteLine($"Player HP before: {player.Hp}");
            Console.WriteLine(trap1.Interact(player));
            Console.WriteLine($"Player HP after: {player.Hp}");

            Console.WriteLine("\n5. Test Trap ApplyDamage:");
            trap1.ApplyDamage(10);
            Console.WriteLine(trap1.Info());
            Console.WriteLine("Try to interact with broken trap:");
            Console.WriteLine(trap1.Interact(player));  

            Console.WriteLine("\n6. Test Enable/Disable:");
            door1.Disable();
            Console.WriteLine(door1.Info());
            door1.Enable();
            Console.WriteLine(door1.Info());

            
            Console.ReadKey();
        }
    }
}
