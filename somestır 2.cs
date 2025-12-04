using System;

namespace Самостоятельная 2
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
            Console.Write($"Name: {name}, Cost: {buildCost}, Production: {production} per minute");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Building[] buildings = new Building[2];

            buildings[0] = new Building("Farm", 15, 10);
            buildings[1] = new Building("Mine", 25, 50);

            Console.Write("Buildings: ");
            buildings[0].DisplayInfo();
            Console.Write(" | ");
            buildings[1].DisplayInfo();
            Console.WriteLine(); 
        }
    }
}