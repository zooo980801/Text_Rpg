using System.Buffers.Text;
using System.Xml.Linq;
using Amazon.S3.Model;
using TextRpg;
using static System.Formats.Asn1.AsnWriter;

namespace TextRpg
{
    internal class FirstSceneLoad
{
    static void Main(string[] args)
    {
        fistScene scene = new fistScene();
        scene.First(); // 이름만 받음

        Stats stats = new Stats();
        stats.GenerateStats(); // 랜덤 스탯 생성

        GameManagement gameManagement = new GameManagement(scene.PlayerName, stats);
        Stage stage = new Stage(scene.PlayerName, stats, gameManagement);
        stage.getSelect(); // 게임 시작
    }
}
}
