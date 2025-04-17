using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using TextRpg;

namespace TextRpg
{


    public class Stage
    {
        private GameManagement gameManagement;
        private string PlayerName;  // 필드로 추가
        private Stats stats;
        private MonsterList monsterList;
        private DungeonManager dungeonManager;
        private Shop shop;
        private Inventory inventory;

        public Stage(string playerName, Stats stats, GameManagement gameManagement)
        {
            PlayerName = playerName;
            this.stats = stats;
            this.gameManagement = gameManagement;
            this.monsterList = new MonsterList();
            this.dungeonManager = new DungeonManager(stats, monsterList, this);
            this.dungeonManager.SetBackSelectAction(getSelect);
            this.inventory = new Inventory(stats, () => getSelect());

            this.shop = new Shop();
            this.shop.SetStage(this, stats, this.inventory);  // Stage와 Stats 객체를 Shop에 전달
        }


        public void getSelect() 
        {
            while (true) 
            {
                Console.Clear();
                Console.WriteLine($"안녕하세요, {PlayerName}님!\n");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기\n");
                Console.WriteLine("2. 스탯 분배\n");
                Console.WriteLine("3. 인벤토리\n");
                Console.WriteLine("4. 상점\n");
                Console.WriteLine("5. 던전 입장\n");
                Console.WriteLine("6. 휴식하기 \n");
                Console.WriteLine("7. 게임 저장\n");
                Console.WriteLine("8. 게임 불러오기\n");
                Console.WriteLine("9. 게임 종료\n");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.\n");

                if (int.TryParse(Console.ReadLine(), out int selectInput)) 

                {
                    if (selectInput >= 1 && selectInput <= 9)
                    {
                        switch (selectInput)
                        {
                            case 1:
                                PlayerInfo();
                                break;
                            case 2:
                                SharePoint();
                                break;
                            case 3:
                                inventory.ManageInventory();
                                break;
                            case 4:
                                shop.Store();
                                break;
                            case 5:
                                dungeonManager.EnterDungeon();
                                break;
                            case 6:
                                Rest();
                                break;
                            case 7:
                                gameManagement.SelectSaveSlot();
                                break;
                            case 8:
                                gameManagement.SelectLoadSlot();
                                break;
                            case 9:
                                gameManagement.ExitGame(); 
                                break;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. '1'에서 '9' 사이의 숫자를 입력하세요.");
                        Thread.Sleep(1500);
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. '1' 에서 '9' 사이의 숫자를 입력하세요.");
                    // 다시 선택

                    Thread.Sleep(1500);
                }
            }
        }

        public void SharePoint()
        {
            while (true)
            {
                Console.Clear();
                stats.StatusWindow();  // Stats 클래스로부터 스탯을 출력

                Console.Write("입력: ");
                string? input = Console.ReadLine();

                if (input == "0") // '0'을 눌러서 나가면 BackSelect() 실행
                {
                    getSelect();  // BackSelect()에서 '0'을 눌렀을 때 바로 종료
                    break;  // SharePoint()에서 루프 종료
                }

                stats.DistributeStat(input!); // Stats 클래스에서 스탯 분배
            }
        }

        public void PlayerInfo()  
        {
            Console.Clear(); 
            Console.WriteLine("\n[ 플레이어 스탯 ]\n");
            Console.WriteLine($" Lv. {stats.Level}\n"); 
            Console.WriteLine($" 이름 : {PlayerName} \n");
            Console.WriteLine($" 힘 : {stats.Str}\n");
            Console.WriteLine($" 공격력 : {stats.Atk}\n");
            Console.WriteLine($" 민첩 : {stats.Dex}\n");
            Console.WriteLine($" 방어력 : {stats.Armor}\n");
            Console.WriteLine($" 회피율: {stats.Avoidance}%\n");
            Console.WriteLine($" 지능 : {stats.Int}\n");
            Console.WriteLine($" 체력 : {stats.Hp}\n");
            Console.WriteLine($" 경험치 : {stats.CurrentExp} / {stats.MaxExp}\n");
            Console.WriteLine($" Gold : {stats.Gold} G\n");
            BackSelect();
        }

        public void Rest()
        {
            Console.Clear();

            // 500골드를 소모하여 휴식을 진행할 것인지 확인
            if (stats.Gold >= 500)
            {
                Console.WriteLine($"500G 를 내면 체력을 회복 할 수 있습니다.\n보유골드: {stats.Gold} G");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");

                string? input = Console.ReadLine()?.ToLower();  

                if (input == "1")
                {
                    // 회복 진행 바 출력
                    int total = 20; // 총 바 길이 (20칸)
                    for (int i = 0; i <= total; i++)
                    {
                        int percent = (i * 100) / total;
                        string bar = new string('■', i).PadRight(total, ' '); // 바 생성
                        Console.Write($"\r[{bar}] {percent}%");
                        Thread.Sleep(100); // 간격 시간
                    }

                    // HP와 MP를 최대값으로 회복
                    stats.Hp = stats.MaxHp;
                    stats.Mp = stats.MaxMp;

                    // 500골드 차감
                    stats.Gold -= 500;

                    Console.WriteLine(); // 줄바꿈
                    Console.WriteLine("HP와 MP가 완전히 회복되었습니다.");
                    Console.WriteLine($"현재 HP: {stats.Hp}/{stats.MaxHp}");
                    Console.WriteLine($"현재 MP: {stats.Mp}/{stats.MaxMp}");
                    Console.WriteLine($"현재 Gold: {stats.Gold} G");

                    Thread.Sleep(1000);
                }
                else if (input == "0")
                {
                    Console.WriteLine("휴식을 취하지 않습니다.");
                    Thread.Sleep(1500);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. '1' 또는 '0'을 입력하세요.");
                    Thread.Sleep(1500);
                }
            }
            else
            {
                Console.WriteLine("골드가 부족합니다. 500 골드가 필요합니다.");
                Thread.Sleep(1500);
            }

            getSelect();
        }



        public void BackSelect()
        {
            Console.WriteLine("메인 메뉴로 돌아가려면 '0'을 누르세요.");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int userInput))
                {
                    if (userInput == 0)
                    {
                        getSelect(); // 메인 메뉴로 돌아가는 함수
                        return;  // 루프 종료 후 SharePoint로 돌아가도록
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 숫자만 입력하세요.");
                    }
                }
            }
        }

    }
}