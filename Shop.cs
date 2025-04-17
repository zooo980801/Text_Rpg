using TextRpg;

public class Shop
{
    private Stage? stage;
    private Stats? stats;
    private Inventory? inventory;

    public void SetStage(Stage stage, Stats stats, Inventory inventory)
    {
        this.stage = stage;
        this.stats = stats;
        this.inventory = inventory;
    }

    // 상점에서 사용할 아이템 리스트
    private List<Itemlist> items;

    public Shop()
    {
        // 아이템 리스트 초기화
        items = new List<Itemlist>
        {
            new Itemlist(" 1번 아이템", "방어력 +1", 0, 2, 1000),
            new Itemlist(" 2번 아이템", "방어력 +2", 0, 6, 1500),
            new Itemlist(" 3번 아이템", "방어력 +3", 0, 10, 3000),
            new Itemlist(" 4번 아이템", "공격력 +1", 2, 0, 500),
            new Itemlist(" 5번 아이템", "공격력 +2", 5, 0, 1000),
            new Itemlist(" 6번 아이템", "공격력 +3", 7, 0, 2000)
        };
    }

    public void Store() // 3. 상점
    {
        Console.Clear();
        Console.WriteLine("[ 상점 ]\n보유 중인 아이템을 관리 할 수 있습니다.\n");
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{stats?.Gold} G\n");

        // 아이템 목록 출력
        Console.WriteLine("[ 아이템 목록 ]");
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            Console.WriteLine($"{i + 1}. {item.ItemName} | {item.ToolTip} | {item.Price}G{(item.Purchase ? " - 구매 완료" : "")}");
        }

        Console.WriteLine();
        Console.WriteLine(" 원하시는 행동을 입력해주세요.\n");
        Console.WriteLine(" 0. 나가기\n 1. 아이템 구매\n 2.아이템 판매");

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int storeInput)) // 0,1 중에 택1
            {
                switch (storeInput)
                {
                    case 0:
                        stage?.getSelect(); // 0. 나가기
                        break;
                    case 1:
                        BuyItem(); // 3-1. 아이템 구매
                        break;
                    case 2:
                        SellItem();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 유효한 번호를 입력하세요.\n");
                        continue; // 다시 루프
                }
            }
        }
    }

    public void BuyItem() // 3-1. 아이템 구매
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[ 상점아이템 구매 ]\n보유하신 골드로 아이템을 구매 할 수 있습니다.\n");
            Console.WriteLine("[보유 골드] {0} G\n", stats?.Gold);

            // 아이템 목록 출력
            Console.WriteLine("[ 아이템 목록 ]");
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                // 이미 구매한 아이템은 구매할 수 없도록 표시
                Console.WriteLine($"{i + 1}. {item.ItemName} | {item.ToolTip} | {item.Price}G | 보유 수량: {item.Quantity} {(item.Purchase ? "- 구매 완료" : "")}");
            }

            Console.WriteLine("\n구매하실 아이템의 번호를 입력하세요. 0은 뒤로 가기 입니다.");

            if (int.TryParse(Console.ReadLine(), out int itemInput) && itemInput >= 0 && itemInput <= items.Count)
            {
                if (itemInput == 0)
                {
                    Store(); // 나가기
                    break;
                }

                var selectedItem = items[itemInput - 1];  // 1번 선택 -> 0번 인덱스

                // 이미 구매한 아이템은 구매할 수 없도록 체크
                if (selectedItem.Purchase)
                {
                    Console.WriteLine("이미 구매한 아이템입니다. 다시 선택해주세요.\n");
                    Thread.Sleep(1000);
                    continue;  // 이미 구매한 아이템은 재구매할 수 없으므로 루프를 계속 진행
                }

                // 아이템 상태 확인 및 처리
                if (stats?.Gold >= selectedItem.Price)
                {
                    Console.WriteLine("구매를 완료했습니다.\n");
                    stats.Gold -= selectedItem.Price;
                    selectedItem.Quantity++; // 수량 증가
                    selectedItem.Purchase = true; // 구매 완료 처리
                    inventory?.AddItem(selectedItem); // 인벤토리에 아이템 추가

                    // 구매 후 화면 갱신
                    Console.Clear(); // 화면 초기화
                    Console.WriteLine("[ 상점 ]\n아이템 구매가 완료되었습니다.\n");
                    Thread.Sleep(1000);
                    Console.WriteLine($"[보유 골드] {stats.Gold} G\n");

                    // 갱신된 아이템 목록 출력
                    Console.WriteLine("[ 아이템 목록 ]");
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        Console.WriteLine($"{i + 1}. {item.ItemName} | {item.ToolTip} | {item.Price}G | 보유 수량: {item.Quantity} {(item.Purchase ? "- 구매 완료" : "")}");
                    }

                    // 다시 상점 메뉴로 안내
                    Console.WriteLine("\n원하시는 행동을 입력해주세요.\n");
                    Console.WriteLine(" 0. 나가기\n 1. 아이템 구매\n 2. 아이템 판매");

                    break; // 아이템을 구매한 후 상점 화면을 다시 띄운 뒤 루프 종료
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.\n");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 아무 키나 누르면 돌아갑니다.");
            }

            Console.WriteLine("\n아무 키나 누르면 돌아갑니다.");
            Console.ReadKey(); // 키를 누를 때까지 대기
        }
    }





    public void SellItem() // 3-2. 아이템 판매
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[ 상점아이템 판매 ]\n보유하신 아이템을 판매할 수 있습니다.\n");
            Console.WriteLine("[보유 골드] {0} G\n", stats?.Gold);

            // 판매 가능한 아이템 목록 출력 (구매한 아이템만)
            Console.WriteLine("[ 보유 아이템 목록 ]");
            List<Itemlist> ownedItems = items.Where(item => item.Quantity > 0).ToList(); // 구매한 아이템만 필터링
            if (ownedItems.Count == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.\n");
                break;
            }

            for (int i = 0; i < ownedItems.Count; i++)
            {
                var item = ownedItems[i];
                Console.WriteLine($"{i + 1}. {item.ItemName} | {item.ToolTip} | 판매 가격: {item.Price / 2}G");  // 판매가는 원래 가격의 절반
            }

            Console.WriteLine("\n판매할 아이템의 번호를 입력하세요. 0은 뒤로 가기 입니다.");

            // 아이템 번호 입력 처리
            if (int.TryParse(Console.ReadLine(), out int itemInput) && itemInput >= 0 && itemInput <= ownedItems.Count)
            {
                if (itemInput == 0)
                {
                    Store(); // 나가기
                    break;
                }

                var selectedItem = ownedItems[itemInput - 1];  // 1번 선택 -> 0번 인덱스

                // 아이템 판매 처리
                Console.WriteLine($"{selectedItem.ItemName}을(를) 판매합니다.\n");
                stats!.Gold += selectedItem.Price / 2;
                selectedItem.Quantity--; // 수량 감소
                inventory?.RemoveItem(selectedItem); // 인벤토리에서 아이템 제거

                // 판매 후 'Purchase' 상태 리셋 (다시 구매 가능)
                selectedItem.Purchase = false; // 판매한 아이템을 다시 구매할 수 있게 설정

                Console.WriteLine($"판매가 완료되었습니다. 현재 골드: {stats?.Gold}G\n");

                // 판매 후 다시 상점 화면으로 돌아가기
                Store();
                break; // 판매 완료 후 상점 메뉴로 돌아가게 되므로 루프 종료
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 아무 키나 누르면 돌아갑니다.");
            }

            Console.WriteLine("\n아무 키나 누르면 돌아갑니다.");
            Console.ReadKey(); // 키를 누를 때까지 대기
        }
    }

}