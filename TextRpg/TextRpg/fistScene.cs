using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TextRpg
{
    internal class fistScene
    {
        public string PlayerName { get; set; } = ""; // 기본값 설정으로 null 방지

        public void First()
        {
            Console.WriteLine("용사의 시험에 오신 것을 환영합니다.\n");
            Console.Write("당신의 이름은 무엇입니까? : ");

            string? input = Console.ReadLine();
            PlayerName = string.IsNullOrWhiteSpace(input) ? "이름없음" : input;

            Console.WriteLine($"\n당신의 이름은 {PlayerName}입니다.");

            Thread.Sleep(2000);
        }
    }
}