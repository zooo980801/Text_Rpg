using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace TextRpg
{
    public class Stats
    {

        public Random rnd = new Random();
        public int Level;
        public int Str;
        public int Int;
        public int Dex;
        public int Str_power;
        public int BaseHp = 50;
        public int BaseMp = 50;
        public int BaseAr = 10;
        public int Base_Str_power;
        public int Armor;
        public int Hp;
        public int MaxHp;
        public int Mp;
        public int MaxMp;
        public int Exp;
        public int Point;
        public int CurrentExp;
        public int MaxExp;
        public int Gold;
        public int BaseExpIncrement;

        public int statLimit = 20;

        public Stats(bool autoGenerate = true)
        {
            Level = 1;
            CurrentExp = 0;
            MaxExp = 10;
            Point = 0;
            Gold = 20000;
            BaseExpIncrement = 10;
            Hp = 100 + (Str * 3); 
            MaxHp = BaseHp + (Str * 4); 
            Mp = 150 + (Int * 4);
            MaxMp = BaseMp + (Int * 4);

            if (autoGenerate)
            {
                RandomStats();
                FirstStats(); 
            }

            Calculate();
        }


        public int Atk
        {
            get { return (int)(Str * 1.5); }
        }
        public int IntAtk
        {
            get { return (int)(Int * 2); }
        }
        public int Avoidance
        {
            get { return (int)(Dex / 2);  } 
        }


        public void ShuffleStats()
        {
            int[] stats = { Str, Int, Dex };
            for (int i = 0; i < stats.Length; i++)
            {
                int j = rnd.Next(i, stats.Length);
                int temp = stats[i];
                stats[i] = stats[j];
                stats[j] = temp;
            }

            Str = stats[0];
            Int = stats[1];
            Dex = stats[2];
        }
        private void RandomStats()
        {
            int remaining = statLimit;

            // 0부터 남은 값까지 중 랜덤값 부여
            Str = rnd.Next(0, remaining + 1);
            remaining -= Str;

            Int = rnd.Next(0, remaining + 1);
            remaining -= Int;


            Dex = rnd.Next(0, remaining + 1);
            remaining -= Dex; 
            // 순서를 매번 섞어서 특정 능력치가 항상 높은 걸 방지할 수도 있음
            ShuffleStats();
        }
        public void FirstStats()
        {
            Console.Clear();

            Calculate();
            Hp = MaxHp;
            Mp = MaxMp;

            Console.WriteLine("=== 랜덤 캐릭터 스탯 생성 ===");
            Console.WriteLine($"힘(Str): {Str}");
            Console.WriteLine($"공격력(Atk): {Atk}");
            Console.WriteLine($"지능(Int): {Int}");
            Console.WriteLine($"민첩(Dex): {Dex}");
            Console.WriteLine($"방어력(Armor): {Armor}");
            Console.WriteLine($"총합: {Str + Int + Dex}/{statLimit}");
            Console.WriteLine($"체력(Hp): {Hp}/{MaxHp}");
            Console.WriteLine($"마나(Mp): {Mp}/{MaxMp}");
            Console.WriteLine($"회피율: {Avoidance}%");
            Console.WriteLine($"골드(Gold): {Gold}");
        }

        public void LevelUp()
        {
            while (CurrentExp >= MaxExp)  // EXP가 계속 넘는 동안 반복
            {
                CurrentExp -= MaxExp;      // 초과 EXP는 다음 레벨로 이월
                Level++;                   // 레벨 증가
                MaxExp += BaseExpIncrement; // MaxExp도 점점 증가
                Point += 3;                // 포인트 지급

                Console.WriteLine($"레벨업! 현재 레벨: {Level}, 포인트: {Point}");
                Console.WriteLine($"현재 EXP: {CurrentExp}/{MaxExp}");
            }
        }

        public void DistributeStat(string input)
        {
            if (Point <= 0)
            {
                Console.WriteLine("분배할 포인트가 없습니다.");
                Thread.Sleep(1000);
                return;
            }

            switch (input)
            {
                case "1":
                    Str += 1;
                    Point -= 1;
                    Console.WriteLine("힘이 1 올랐습니다!");
                    break;
                case "2":
                    Int += 1;
                    Point -= 1;
                    Console.WriteLine("지능이 1 올랐습니다!");
                    break;
                case "3":
                    Dex += 1;
                    Point -= 1;
                    Console.WriteLine("민첩이 1 올랐습니다!");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }

            Calculate(); 
            Thread.Sleep(500);
        }
        public void StatusWindow()
        {
                Console.Clear();
                Console.WriteLine("올리고 싶은 스탯을 선택하세요:");
                Console.WriteLine($"남은 포인트 : {Point}");
                Console.WriteLine($"1. 힘 (Str) : 현재스탯 : {Str}");
                Console.WriteLine($"2. 지능 (Int) : 현재스탯 : {Int}");
                Console.WriteLine($"3. 민첩 (Dex) : 현재스탯 : {Dex}");
                Console.WriteLine($"0. 나가기");
        }

        public void GainReward(int exp, int gold)
        {
            CurrentExp += exp; 
            Console.WriteLine($"EXP가 {exp}만큼 증가! 현재 EXP: {CurrentExp}");
            Gold += gold;

            Console.WriteLine($"Gold가 {gold}만큼 증가! 현재 Gold: {Gold}");

            // 레벨업 체크
            LevelUp();
        }
        public void Calculate()
        {
            MaxHp = BaseHp + (Str * 2);
            MaxMp = BaseMp + (Int * 1);
            Armor = BaseAr + (int)(Dex * 0.5);
            Str_power = Base_Str_power + (Str * 2);

            // 현재 HP/MP를 Max로 맞추기
            if (Hp > MaxHp) Hp = MaxHp;
            if (Mp > MaxMp) Mp = MaxMp;

            // 초기화 시에는 HP/MP를 Max로 맞춰주기
            if (Hp == 0) Hp = MaxHp;
            if (Mp == 0) Mp = MaxMp;
        }

        public void GenerateStats(bool showWindow = true)
        {
            while (true)
            {
                RandomStats();
                Calculate();

                if (showWindow)
                {
                    Console.Clear();
                    FirstStats();
                }

                Console.WriteLine("다시 생성하려면 Enter, 확정하시하려면 Q 입력");
                string? input = Console.ReadLine();
                Console.WriteLine($"입력된 값: [{input}]");

                if (input != null && input.ToLower() == "q")
                {
                    break;  // "Q" 입력시 루프 종료
                }

                // "Enter" 입력시 루프 계속
                else if (input == "")
                {
                    // 아무 것도 하지 않고 루프가 계속됩니다
                }
            }
        }

    }
}
