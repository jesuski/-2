using System;

namespace SettlementGame
{
    class Building
    {
        private string name;
        private int buildCost;
        private int production;

        public string Name { get { return name; } }
        public int BuildCost { get { return buildCost; } }
        public int Production { get { return production; } }

        public Building(string name, int buildCost, int production)
        {
            this.name = name;
            this.buildCost = buildCost;
            this.production = production;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {name}, Cost: {buildCost}, Production: {production}/min");
        }
    }

    class Settlement
    {
        private Building[] buildings;
        private int budget;
        private int buildingCount; 

        public Settlement(int initialBudget)
        {
            buildings = new Building[5]; 
            budget = initialBudget;
            buildingCount = 0;
        }

        
        public void AddBuilding(Building building)
        {
            
            if (buildingCount >= buildings.Length)
            {
                Console.WriteLine("No free slots for new building.");
                return;
            }

            
            if (budget < building.BuildCost)
            {
                Console.WriteLine($"Not enough money for {building.Name}. Need: {building.BuildCost}, Have: {budget}");
            
                
                
                return;
            }

           
            buildings[buildingCount] = building;
            buildingCount++;
            budget -= building.BuildCost;
            Console.WriteLine($"Building {building.Name} added successfully. Remaining budget: {budget}");
        }

        public int GetTotalProduction()
        {
            int total = 0;
            for (int i = 0; i < buildingCount; i++)
            {
                total += buildings[i].Production;
            }
            return total;
        }

        public int GetBudget()
        {
            return budget;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Settlement Management System ===");

            Settlement mySettlement = new Settlement(500);
            Console.WriteLine($"Starting budget: {mySettlement.GetBudget()}");

            Building farm = new Building("Farm", 100, 10);
            Building mine = new Building("Mine", 200, 25);
            Building sawmill = new Building("Sawmill", 150, 15);
            Building expensive = new Building("Factory", 450, 50);

            Console.WriteLine("\n--- Adding buildings ---");
            mySettlement.AddBuilding(farm);
            mySettlement.AddBuilding(mine);
            mySettlement.AddBuilding(sawmill);
            mySettlement.AddBuilding(expensive); 

           
            mySettlement.AddBuilding(new Building("Quarry", 100, 20));

            Console.WriteLine($"\nTotal production of settlement: {mySettlement.GetTotalProduction()}/min");
            Console.WriteLine($"Final budget: {mySettlement.GetBudget()}");
        }
    }
}
