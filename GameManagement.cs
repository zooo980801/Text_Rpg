using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;

namespace TextRpg
{
    public class GameManagement
    {
        private Stats stats;  // 이미 선언되어 있음
        private Stage stage;
        private string playerName;
        private Action backSelectAction;

        public void Presszero()
        {
            // 실행 중 도망가면
            backSelectAction?.Invoke();
            stage.getSelect();  // 던전 종료 후 메인 메뉴로 돌아가도록 호출
        }

        public GameManagement(string playerName, Stats stats)
        {
            this.stats = stats;
            this.playerName = playerName;
            this.stage = new Stage(playerName, stats, this);
            this.backSelectAction = stage.getSelect;
        }
        public void SelectSaveSlot()
        {
            Console.Clear();
            Console.WriteLine("어디에 게임을 저장하시겠습니까?");
            Console.WriteLine("1. 소켓 1");
            Console.WriteLine("2. 소켓 2");
            Console.WriteLine("3. 소켓 3");
            Console.WriteLine("0. 취소");

            string? input = Console.ReadLine();

            // 소켓 번호에 맞춰 SaveGame 호출
            switch (input)
            {
                case "1":
                    SaveGame(1); // 소켓 1에 저장
                    break;
                case "2":
                    SaveGame(2); // 소켓 2에 저장
                    break;
                case "3":
                    SaveGame(3); // 소켓 3에 저장
                    break;
                case "0":
                    Console.WriteLine("저장이 취소되었습니다.");
                    Thread.Sleep(1500);
                    stage.getSelect();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1500);
                    break;
            }
        }


        public void SelectLoadSlot()
        {
            Console.Clear();
            Console.WriteLine("어디서 게임을 불러오시겠습니까?");
            Console.WriteLine("1. 소켓 1");
            Console.WriteLine("2. 소켓 2");
            Console.WriteLine("3. 소켓 3");
            Console.WriteLine("0. 취소");

            string? input = Console.ReadLine();

            // 소켓 번호에 맞춰 LoadGame 호출
            switch (input)
            {
                case "1":
                    LoadGame(1); // 소켓 1에서 불러오기
                    break;
                case "2":
                    LoadGame(2); // 소켓 2에서 불러오기
                    break;
                case "3":
                    LoadGame(3); // 소켓 3에서 불러오기
                    break;
                case "0":
                    Console.WriteLine("불러오기 취소되었습니다.");
                    Thread.Sleep(1500);
                    stage.getSelect();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1500);
                    break;
            }
        }

        public void SaveGame(int slot) // 게임 저장
        {
            Console.Clear();

            // 데이터 계산 후 저장
            stats.Calculate(); // 스탯 계산
            string filePath = $"savedGame{slot}.txt";

            string saveData = $"Level:{stats.Level}\n" +
                              $"Str:{stats.Str}\n" +
                              $"Int:{stats.Int}\n" +
                              $"Dex:{stats.Dex}\n" +
                              $"Hp:{stats.Hp}/{stats.MaxHp}\n" +
                              $"Mp:{stats.Mp}/{stats.MaxMp}\n" +
                              $"Gold:{stats.Gold}\n" +
                              $"CurrentExp:{stats.CurrentExp}\n" +
                              $"MaxExp:{stats.MaxExp}\n" +
                              $"Point:{stats.Point}\n";

            try
            {
                File.WriteAllText(filePath, saveData);
                Console.WriteLine($"게임이 {slot}번 파일에 저장되었습니다!");
                Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 저장에 실패했습니다. 오류: {ex.Message}");
                Thread.Sleep(1500);
            }

            stage.BackSelect(); // 메인 메뉴로 돌아감
        }

        public void LoadGame(int slot) // 6. 게임 저장
        {
            Console.Clear();

            // 파일 경로 설정
            string filePath = $"savedGame{slot}.txt";

            if (File.Exists(filePath)) // 파일이 존재하는지 확인
            {
                try
                {
                    // 파일에서 모든 데이터를 읽기
                    string[] saveData = File.ReadAllLines(filePath);

                    // 저장된 데이터에서 각 항목을 추출하여 Stats 클래스의 필드에 할당
                    foreach (string line in saveData)
                    {
                        string[] splitLine = line.Split(':');
                        string key = splitLine[0];
                        string value = splitLine[1];

                        switch (key)
                        {
                            case "Level":
                                stats.Level = int.Parse(value);
                                break;
                            case "Str":
                                stats.Str = int.Parse(value);
                                break;
                            case "Int":
                                stats.Int = int.Parse(value);
                                break;
                            case "Dex":
                                stats.Dex = int.Parse(value);
                                break;
                            case "Hp":
                                stats.Hp = int.Parse(value.Split('/')[0]); // 현재 HP만 읽기
                                stats.MaxHp = int.Parse(value.Split('/')[1]); // 최대 HP
                                break;
                            case "Mp":
                                stats.Mp = int.Parse(value.Split('/')[0]); // 현재 MP만 읽기
                                stats.MaxMp = int.Parse(value.Split('/')[1]); // 최대 MP
                                break;
                            case "Gold":
                                stats.Gold = int.Parse(value);
                                break;
                            case "CurrentExp":
                                stats.CurrentExp = int.Parse(value);
                                break;
                            case "MaxExp":
                                stats.MaxExp = int.Parse(value);
                                break;
                            case "Point":
                                stats.Point = int.Parse(value);
                                break;
                            default:
                                break;
                        }
                    }

                    Console.WriteLine($"{slot}번째 게임이 성공적으로 불러와졌습니다!");
                    Thread.Sleep(1500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"게임 불러오기에 실패했습니다. 오류: {ex.Message}");
                    Thread.Sleep(1500);
                }
            }
            else
            {
                Console.WriteLine("저장된 게임 파일이 없습니다.");
                Thread.Sleep(1500);
            }
            stage.BackSelect(); // 메서드가 끝나면 선택 화면으로 돌아감
        }

        public void ExitGame()
        {
            Console.Clear();
            Console.WriteLine("게임을 종료합니다...");
            Thread.Sleep(1500);  // 1초 대기 후 종료
            Environment.Exit(0);  // 프로그램 종료
        }
        
    }
}
