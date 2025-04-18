using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class DungeonManager
    {
        private Stats stats;
        private MonsterList monsterList;
        private Stage stage;
        private Action? backSelectAction;

        public DungeonManager(Stats stats, MonsterList monsterList, Stage stage)
        {
            this.stats = stats ?? throw new ArgumentNullException(nameof(stats));
            this.monsterList = monsterList ?? throw new ArgumentNullException(nameof(monsterList));
            this.stage = stage ?? throw new ArgumentNullException(nameof(stage));
        }
        public void SetStage(Stage stage)
        {
            this.stage = stage;
        }

        public void SetBackSelectAction(Action action)
        {
            backSelectAction = action;
        }
        public void SomeRunMethod()
        {
            // 실행 중 도망가면
            backSelectAction?.Invoke();
        }

        private void BattleUI(Monster monster)
        {
            Console.Clear();
            Console.WriteLine("=== 몬스터 등장! ===");
            monster.DisplayMonsterInfo();

            Console.WriteLine("\n======== 전투 중 ========");
            Console.WriteLine($"몬스터: {monster.Name} (HP: {Math.Max(0, monster.HP)})"); // HP가 0 미만이 되지 않도록
            Console.WriteLine($"플레이어 HP: {stats.Hp}/{stats.MaxHp}");
            Console.WriteLine($"플레이어 MP: {stats.Mp}/{stats.MaxMp}");
            Console.WriteLine("\n무엇을 할까?");
            Console.WriteLine("1. 물리 공격");
            Console.WriteLine("2. 마법 공격");
            Console.WriteLine("3. 도망가기");
            Console.WriteLine("입력: ");
        }

        public void EnterDungeon()
        {
            string input = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== 지역을 선택하세요 ===");
                Console.WriteLine("1. 숲");
                Console.WriteLine("2. 동굴");
                Console.WriteLine("3. 묘지");
                Console.WriteLine("4. 던전");
                Console.WriteLine("5. 고원");
                Console.WriteLine("6. 용의 둥지");
                Console.WriteLine("메인 메뉴로 돌아가려면 '0'을 누르세요.");
                Console.Write("선택 > ");

                input = Console.ReadLine() ?? string.Empty;

                if (input == "0")
                {
                    stage.getSelect();
                    return;
                }

                if (input is "1" or "2" or "3" or "4" or "5" or "6")
                {
                    break; // 유효한 입력 -> 반복문 탈출
                }

                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Thread.Sleep(1000);
            }

            List<Monster> selectedMonsters = new List<Monster>();

            switch (input)
            {
                case "1": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "슬라임"); break;
                case "2": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "고블린"); break;
                case "3": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "좀비" || m.Name == "스켈레톤"); break;
                case "4": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "미믹" || m.Name == "데빌"); break;
                case "5": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "골렘" || m.Name == "키메라"); break;
                case "6": selectedMonsters = monsterList.Monsters.FindAll(m => m.Name == "드레이크" || m.Name == "용"); break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(500);
                    return;
            }

            if (selectedMonsters.Count == 0)
            {
                Console.WriteLine("오류");
                return;
            }

            Random rnd = new Random();
            var monster = selectedMonsters[rnd.Next(selectedMonsters.Count)];;

            while (monster.HP > 0 && stats.Hp > 0 )
            {
                BattleUI(monster);

                string? choice = Console.ReadLine();

                if (choice != "1" && choice != "2" && choice != "3")
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                    Thread.Sleep(1000); // 잠시 대기 후 다시 선택을 받도록
                    continue; // 잘못된 입력 시 다시 선택하게 합니다.
                }

                switch (choice)
                {
                    case "1":
                        monster.HP -= stats.Atk;
                        Console.WriteLine($"물리 공격! {stats.Atk} 데미지!");
                        break;

                    case "2":
                        monster.HP -= stats.IntAtk;
                        Console.WriteLine($"마법 공격! {stats.IntAtk} 데미지!");
                        break;

                    case "3":
                        if (new Random().Next(100) < 50)
                        {
                            Console.WriteLine("도망 성공!");
                            Thread.Sleep(1000);
                            backSelectAction?.Invoke(); // 델리게이트 사용
                        }
                        else
                        {
                            Console.WriteLine("도망 실패!");
                            Thread.Sleep(1000);
                        }
                        break;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                if (monster.HP > 0)
                {
                    int damage = Math.Max(1, monster.Power - stats.Armor / 2);
                    stats.Hp -= damage;

                    Console.WriteLine($"{monster.Name}의 공격! {damage} 데미지를 입었습니다.");
                    Thread.Sleep(1000);

                    if (stats.Hp <= 0)
                    {
                        Console.WriteLine("플레이어 사망... 게임 오버");
                        return;
                    }
                }

                Thread.Sleep(1500);
                Console.Clear();
            }

            Console.WriteLine($"{monster.Name} 처치 완료!");
            stats.GainReward(monster.ExpReward, monster.GoldReward);

            // 전투 종료 후 계속할지 선택
            Console.WriteLine("\n전투가 끝났습니다.");
            Console.WriteLine("1. 계속 진행");
            Console.WriteLine("2. 집에 가기");
            Console.Write("입력: ");
            string? endChoice = Console.ReadLine();

            if (endChoice == "2")
            {
                Console.WriteLine("집에 가는 중...");
                Thread.Sleep(1000);
                stage.getSelect(); // 메인 메뉴로 돌아가기
            }
            else if (endChoice == "1")
            {
                Console.WriteLine("계속 진행합니다...");
                // 이곳에서 추가적인 전투 또는 다른 행동을 구현할 수 있습니다.
                EnterDungeon();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Thread.Sleep(1000);
            }
        }
    }
    
}
