using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Monster
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int ExpReward { get; set; }
        public int GoldReward { get; set; }
        public int HP { get; set; }
        public int Power { get; set; }

        public Monster(string name, int level, int expReward, int goldReward, int hp, int power)
        {
            Name = name;
            Level = level;
            ExpReward = expReward;
            GoldReward = goldReward;
            HP = hp;
            Power = power;
        }

        public void DisplayMonsterInfo()
        {
            Console.WriteLine($"{Name} (Lv {Level})");
            Console.WriteLine($"EXP 보상: {ExpReward}");
            Console.WriteLine($"골드 보상: {GoldReward}");
            Console.WriteLine($"HP: {HP}");
            Console.WriteLine($"POWER: {Power}");
            Console.WriteLine();
        }
    }
    public class MonsterList
    {
        public List<Monster> Monsters { get; set; }

        public MonsterList()
        {
            Monsters = new List<Monster>
            {
                new Monster("슬라임", 2, 20, 200, 100, 6),
                new Monster("고블린", 4, 40, 400, 200, 12),
                new Monster("좀비", 6, 60, 600, 300, 18),
                new Monster("스켈레톤", 80, 80, 800, 400, 24),
                new Monster("미믹", 10, 100, 1000, 500, 30),
                new Monster("데빌", 15, 150, 1500, 750, 45),
                new Monster("골렘", 20, 200, 2000, 1000, 60),
                new Monster("키메라", 30, 300, 3000, 1500, 90),
                new Monster("키메라", 40, 400, 4000, 2000, 120),
                new Monster("용", 50, 500, 5000, 2500, 150)
            };
        }

        // 몬스터 목록 출력 메서드
        public void DisplayAllMonsters()
        {
            foreach (var monster in Monsters)
            {
                monster.DisplayMonsterInfo();
            }
        }
        
    }
}
