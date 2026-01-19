using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            AchievementSystem system = new AchievementSystem();

            system.AddAchievement(new Achievement
            {
                Id = 1,
                Name = "first steps",
                Description = "game starting"
            });

            system.AddAchievement(new Achievement
            {
                Id = 2,
                Name = "Коллекционер",
                Description = "Соберите 10 предметов"
            });

            system.UnlockAchievement(1);

            Console.WriteLine(system.GetProgress());
        }
    }

    class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
    }

    class AchievementSystem
    {
        private List<Achievement> achievements = new List<Achievement>();

        public void AddAchievement(Achievement achievement)
        {
            if (achievement != null)
                achievements.Add(achievement);
        }

        public bool UnlockAchievement(int achievementId)
        {
            foreach (var achievement in achievements)
            {
                if (achievement.Id == achievementId)
                {
                    achievement.IsUnlocked = true;
                    return true;
                }
            }
            return false;
        }

        public Achievement GetAchievement(int achievementId)
        {
            foreach (var achievement in achievements)
            {
                if (achievement.Id == achievementId)
                    return achievement;
            }
            return null;
        }

        public int GetUnlockedCount()
        {
            int count = 0;
            foreach (var achievement in achievements)
            {
                if (achievement.IsUnlocked)
                    count++;
            }
            return count;
        }

        public int GetAllUnlocked()
        {
            return achievements.Count;
        }

        public string GetProgress()
        {
            return $"Достижения: {GetUnlockedCount()}/{GetAllUnlocked()}";
        }
    }
}
