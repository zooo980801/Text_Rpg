using TextRpg;

public class Inventory
{
    private Stats stats;
    private Action ReturnToMainMenu;
    //private Stage stage;//
    public List<Itemlist> OwnedItems { get; private set; }

    public List<Itemlist> GetEquippedItems()
    {
        return OwnedItems.Where(item => item.IsEquipped).ToList();
    }
    public Inventory(Stats stats, Action returnToMainMenu)
    {
        this.stats = stats;
        this.ReturnToMainMenu = returnToMainMenu;  // 콜백 저장
        OwnedItems = new List<Itemlist>();
    }

    // 인벤토리 관리 메인
    public void ManageInventory()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[ 인벤토리 ]\n보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]\n");

            if (OwnedItems.Count == 0)
            {
                Console.WriteLine("보유한 아이템이 없습니다.\n");
            }
            else
            {
                for (int i = 0; i < OwnedItems.Count; i++)
                {
                    var item = OwnedItems[i];
                    string equipStatus = item.IsEquipped ? "[E]" : "";
                    Console.WriteLine($"{i + 1}. {equipStatus}{item.ItemName} | {item.ToolTip} | 수량: {item.Quantity}");
                }
            }

            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.WriteLine(" 0. 나가기\n 1. 장착/해제 관리");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                switch (input)
                {
                    case 0:
                        ReturnToMainMenu?.Invoke();
                        return;
                    case 1:
                        ManageEquipment();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
                        break;
                }
            }
        }
    }

    // 장비 관리 (장착 및 해제)
    public void ManageEquipment()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("[ 장착 관리 ]\n어떤 아이템을 장착/해제할까요?\n");

            List<Itemlist> equipableItems = OwnedItems.FindAll(item => item.Quantity > 0);

            if (equipableItems.Count == 0)
            {
                Console.WriteLine("장착 가능한 아이템이 없습니다.\n");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < equipableItems.Count; i++)
            {
                var item = equipableItems[i];
                string equipStatus = item.IsEquipped ? "[E]" : "";
                Console.WriteLine($"{i + 1}. {equipStatus}{item.ItemName} | {item.ToolTip} | 공격력: {item.Attack} | 방어력: {item.Defense}");
            }

            Console.WriteLine("\n장착/해제 하실 아이템 번호를 선택해주세요. (0. 뒤로 가기)");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0) return;

                if (choice > 0 && choice <= equipableItems.Count)
                {
                    EquipConv(equipableItems[choice - 1]);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }
        }
    }

    // 장비전환
    private void EquipConv(Itemlist item)
    {
        if (item.IsEquipped)
        {
            Console.WriteLine($"{item.ItemName}을(를) 장착 해제합니다.");
            if (item.Unequip())
            {
                stats.Str -= item.Attack;
                stats.Armor -= item.Defense;
            }
        }
        else
        {
            Console.WriteLine($"{item.ItemName}을(를) 장착합니다.");
            if (item.Equip())
            {
                stats.Str += item.Attack;
                stats.Armor += item.Defense;
            }
        }
        Console.WriteLine($"현재 공격력: {stats.Str}, 방어력: {stats.Armor}");
    }

    public void AddItem(Itemlist item)
    {
        var existingItem = OwnedItems.Find(i => i.ItemName == item.ItemName);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var newItem = new Itemlist(item.ItemName, item.ToolTip, item.Attack, item.Defense, item.Price)
            {
                Quantity = 1 // 수량을 1로 설정
            };
            OwnedItems.Add(newItem);
        }
    }

    // 아이템 제거
    public void RemoveItem(Itemlist item)
    {
        var existingItem = OwnedItems.Find(i => i.ItemName == item.ItemName);
        if (existingItem != null)
        {
            existingItem.Quantity--;

            if (existingItem.Quantity <= 0)
            {
                OwnedItems.Remove(existingItem);
            }
        }
    }
}
