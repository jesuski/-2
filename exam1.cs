using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam1
{
    class Program
    {
        static void Main(string[] args)
        {
            ExperienceSystem expSystem = new ExperienceSystem();

            Console.WriteLine(expSystem.GetProgress());

            expSystem.AddExperience(150);
            Console.WriteLine(expSystem.GetProgress());

            expSystem.AddExperience(1500);
            Console.WriteLine(expSystem.GetProgress());

            expSystem.AddExperience(150);
            Console.WriteLine(expSystem.GetProgress());
        }
    }

    class ExperienceSystem
    {
        private int currentExp;
        private int currentLevel;
        private int expToNextLevel;

        public ExperienceSystem()
        {
            currentLevel = 1;
            currentExp = 0;
            expToNextLevel = CalculateExpToNextLevel();
        }

        private int CalculateExpToNextLevel()
        {
            return 100 * currentLevel * currentLevel;
        }

        public void AddExperience(int exp)
        {
            if (exp < 0)
            {
                Console.WriteLine("error negative control cannot be added.");
                return;
            }

            currentExp += exp;

            while (currentExp >= expToNextLevel)
            {
                currentExp -= expToNextLevel;
                currentLevel++;
                expToNextLevel = CalculateExpToNextLevel();
            }
        }

        public string GetProgress()
        {
            return $"Уровень {currentLevel}: {currentExp}/{expToNextLevel} XP";
        }
    }
}

