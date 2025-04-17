using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRpg
{
    public class Itemlist
    {
        public string ItemName { get; set; }
        public string ToolTip { get; set; }
        public int Attack { get; set; }  // 공격력
        public int Defense { get; set; } // 방어력
        public int Price { get; set; }   // 가격
        public bool Purchase { get; set; } // 구매 여부
        public int Quantity { get; set; } // 구매한 수량을 기록하는 변수
        public bool IsEquipped { get; set; } // 장착 여부 추가

        // 생성자
        public Itemlist(string itemName, string toolTip, int attack, int defense, int price)
        {
            ItemName = itemName;
            ToolTip = toolTip;
            Attack = attack;
            Defense = defense;
            Price = price;
            Purchase = false;
            IsEquipped = false;
            Quantity = 1;
        }

        public bool Equip()
        {
            if (Quantity > 0 && !IsEquipped) 
            {
                IsEquipped = true;
                return true;
            }
            return false; 
        }

        public bool Unequip()
        {
            if (IsEquipped) 
            {
                IsEquipped = false;
                return true;
            }
            return false; 
        }

    }
}
