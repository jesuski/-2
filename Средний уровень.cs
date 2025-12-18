using System;
using System.Collections.Generic;

namespace kontrolnaya2
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

        public void ShowStatus()
        {
            Console.WriteLine($"Player: HP={Hp}, Access Card={HasAccessCard}, Checkpoint={LastCheckpointId}");
        }
    }

    class Door : GameObject, IInteractable
    {
        private bool requiredAccess;

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
                return "Door opened";
            }
            else
            {
                return "Access denied";
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
            if (amount > 0)
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

    

    
    class Scene
    {
        public List<GameObject> Objects { get; } = new List<GameObject>();
        public Player CurrentPlayer { get; } = new Player();

        public void PrintAll()
        {
            Console.WriteLine("\nAll Objects In Scene");
            foreach (var obj in Objects)
            {
                Console.WriteLine(obj.Info());
            }
        }

        public void PrintOnlyInteractive()
        {
            Console.WriteLine("\n Interactive Objects");
            foreach (var obj in Objects)
            {
                if (obj is IInteractable && obj.IsActive)
                {
                    Console.WriteLine(obj.Info());
                }
            }
        }

        public GameObject FindObjectById(int id)
        {
            foreach (var obj in Objects)
            {
                if (obj.Id == id)
                    return obj;
            }
            return null;
        }
    }

   
    class Light : GameObject, ITriggerable
    {
        private bool isOn = false;

        public Light(string name) : base(name) { }

        public void Trigger()
        {
            isOn = !isOn;
            Console.WriteLine($"{Name} is now {(isOn ? "ON" : "OFF")}");
        }

        public override string Info()
        {
            return $"Light: {Name} (ID:{Id}), Active:{IsActive}, IsOn:{isOn}";
        }
    }

    
    class Button : GameObject, IInteractable
    {
        private ITriggerable target;

        public Button(string name, ITriggerable targetObject) : base(name)
        {
            target = targetObject;
        }

        public string Interact(Player player)
        {
            if (!IsActive)
                return "Button is not active";

            target.Trigger();
            return $"Button {Name} pressed!";
        }

        public override string Info()
        {
            return $"Button: {Name} (ID:{Id}), Active:{IsActive}";
        }
    }

    // 4. Console Menu System
    class GameMenu
    {
        private Scene scene;

        public GameMenu(Scene scene)
        {
            this.scene = scene;
            InitializeScene();
        }

        private void InitializeScene()
        {
            // Create and add objects
            scene.Objects.Add(new Door("Main Entrance", true));
            scene.Objects.Add(new Door("Storage Room", false));
            scene.Objects.Add(new Checkpoint("Starting Area"));
            scene.Objects.Add(new Checkpoint("Boss Room Entrance"));
            scene.Objects.Add(new Trap("booby trap", 150));
            scene.Objects.Add(new Trap("Poison Dart", 75));

           
            Light corridorLight = new Light("Corridor Light");
            scene.Objects.Add(corridorLight);
            scene.Objects.Add(new Button("Light Switch", corridorLight));

           
            scene.CurrentPlayer.HasAccessCard = true;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\n MAIN MENU ");
                Console.WriteLine("1. Show all objects");
                Console.WriteLine("2. Show only interactive objects");
                Console.WriteLine("3. Interact with object by ID");
                Console.WriteLine("4. Disable object by ID");
                Console.WriteLine("5. Enable object by ID");
                Console.WriteLine("6. Show player status");
                Console.WriteLine("0. Exit");
                Console.Write("Your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1.":
                        scene.PrintAll();
                        break;
                    case "2.":
                        scene.PrintOnlyInteractive();
                        break;
                    case "3.":
                        InteractWithObject();
                        break;
                    case "4.":
                        ToggleObject(false);
                        break;
                    case "5.":
                        ToggleObject(true);
                        break;
                    case "6.":
                        scene.CurrentPlayer.ShowStatus();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
            }
        }

        private void InteractWithObject()
        {
            Console.Write("Enter object ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                GameObject obj = scene.FindObjectById(id);
                if (obj != null)
                {
                    // Pattern matching as required
                    if (obj is IInteractable interactable)
                    {
                        string result = interactable.Interact(scene.CurrentPlayer);
                        Console.WriteLine($"Result: {result}");
                    }
                    else
                    {
                        Console.WriteLine("Object is not interactable");
                    }
                }
                else
                {
                    Console.WriteLine("Object not found!");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID!");
            }
        }

        private void ToggleObject(bool enable)
        {
            Console.Write("Enter object ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                GameObject obj = scene.FindObjectById(id);
                if (obj != null)
                {
                    if (enable)
                        obj.Enable();
                    else
                        obj.Disable();
                    Console.WriteLine($"Object {obj.Name} is now {(obj.IsActive ? "active" : "inactive")}");
                }
                else
                {
                    Console.WriteLine("Object not found!");
                }
            }
        }
    }

    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("GAME ENGINE");


            Scene gameScene = new Scene();
            GameMenu menu = new GameMenu(gameScene);

            menu.ShowMenu();

            
           
        }
    }
}