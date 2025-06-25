using FirebaseAdmin;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public string PlayerID { get; set; }
    public string Name { get; set; }
    public int Gold { get; set; }
    public int Score { get; set; }
}

class Program
{
    static FirebaseClient firebase;

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("C:\\dev\\K24GD03NhatTien\\serviceAccountKey.json")
        });

        firebase = new FirebaseClient("https://nhattien1231-8c327-default-rtdb.asia-southeast1.firebasedatabase.app/");

        int choice;
        do
        {
            Console.WriteLine("\n=== MENU CHỨC NĂNG ===");
            Console.WriteLine("1. Thêm người chơi");
            Console.WriteLine("2. Cập nhật Gold");
            Console.WriteLine("3. Cập nhật Score");
            Console.WriteLine("4. Xoá người chơi");
            Console.WriteLine("5. Tạo & hiển thị Top 5 Gold");
            Console.WriteLine("6. Tạo & hiển thị Top 5 Score");
            Console.WriteLine("7. Hiển thị danh sách người chơi");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng (0-7): ");
            if (!int.TryParse(Console.ReadLine(), out choice)) continue;

            switch (choice)
            {
                case 1:
                    await NhapNguoiChoi();
                    break;
                case 2:
                    Console.Write("PlayerID: ");
                    string idGold = Console.ReadLine();
                    Console.Write("Gold mới: ");
                    if (int.TryParse(Console.ReadLine(), out int newGold))
                        await UpdateGold(idGold, newGold);
                    break;
                case 3:
                    Console.Write("PlayerID: ");
                    string idScore = Console.ReadLine();
                    Console.Write("Score mới: ");
                    if (int.TryParse(Console.ReadLine(), out int newScore))
                        await UpdateScore(idScore, newScore);
                    break;
                case 4:
                    Console.Write("PlayerID muốn xoá: ");
                    string idDel = Console.ReadLine();
                    await firebase.Child("Players").Child(idDel).DeleteAsync();
                    Console.WriteLine(" Đã xoá người chơi.");
                    break;
                case 5:
                    await GenerateTopListAsync("Gold", "TopGold");
                    break;
                case 6:
                    await GenerateTopListAsync("Score", "TopScore");
                    break;
                case 7:
                    await HienThiDanhSachNguoiChoi();
                    break;
                case 0:
                    Console.WriteLine(" Tạm biệt!");
                    break;
                default:
                    Console.WriteLine(" Lựa chọn không hợp lệ.");
                    break;
            }

        } while (choice != 0);
    }

    static async Task NhapNguoiChoi()
    {
        Console.Write("PlayerID: ");
        string id = Console.ReadLine();
        Console.Write("Tên: ");
        string name = Console.ReadLine();
        Console.Write("Gold: ");
        int.TryParse(Console.ReadLine(), out int gold);
        Console.Write("Score: ");
        int.TryParse(Console.ReadLine(), out int score);

        var player = new Player
        {
            PlayerID = id,
            Name = name,
            Gold = gold,
            Score = score
        };

        await firebase.Child("Players").Child(id).PutAsync(player);
        Console.WriteLine(" Đã thêm người chơi.");
    }

    static async Task UpdateGold(string playerId, int newGold)
    {
        var player = await firebase.Child("Players").Child(playerId).OnceSingleAsync<Player>();
        if (player != null)
        {
            player.Gold = newGold;
            await firebase.Child("Players").Child(playerId).PutAsync(player);
            Console.WriteLine($" Đã cập nhật Gold cho {playerId}.");
        }
        else Console.WriteLine(" Không tìm thấy người chơi.");
    }

    static async Task UpdateScore(string playerId, int newScore)
    {
        var player = await firebase.Child("Players").Child(playerId).OnceSingleAsync<Player>();
        if (player != null)
        {
            player.Score = newScore;
            await firebase.Child("Players").Child(playerId).PutAsync(player);
            Console.WriteLine($" Đã cập nhật Score cho {playerId}.");
        }
        else Console.WriteLine(" Không tìm thấy người chơi.");
    }

    static async Task GenerateTopListAsync(string field, string nodeName)
    {
        var snapshot = await firebase.Child("Players").OnceAsync<Player>();

        var top5 = snapshot
            .Where(p => p.Object != null)
            .Select(p => p.Object)
            .OrderByDescending(p => field == "Gold" ? p.Gold : p.Score)
            .Take(5)
            .ToList();

        for (int i = 0; i < top5.Count; i++)
        {
            var entry = new
            {
                Index = i + 1,
                PlayerID = top5[i].PlayerID,
                Name = top5[i].Name,
                Gold = top5[i].Gold,
                Score = top5[i].Score,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await firebase.Child(nodeName).Child($"Rank_{i + 1}").PutAsync(entry);
        }

        Console.WriteLine($"\n {(field == "Gold" ? "TOP 5 GOLD" : "TOP 5 SCORE")}:");
        for (int i = 0; i < top5.Count; i++)
        {
            var p = top5[i];
            string only = field == "Gold" ? $"{p.Gold} Gold" : $"{p.Score} Score";
            Console.WriteLine($"#{i + 1} {p.Name} - {only}");
        }

        Console.WriteLine($" Đã cập nhật node {nodeName}.\n");
    }

    static async Task HienThiDanhSachNguoiChoi()
    {
        var snapshot = await firebase.Child("Players").OnceAsync<Player>();

        var players = snapshot
            .Where(p => p.Key != null && p.Object != null)
            .Select(p => p.Object)
            .ToList();

        if (players.Count == 0)
        {
            Console.WriteLine(" Danh sách người chơi trống.");
            return;
        }

        Console.WriteLine("\n Danh sách người chơi:");
        foreach (var p in players)
        {
            Console.WriteLine($"- {p.PlayerID} | {p.Name} | Gold: {p.Gold} | Score: {p.Score}");
        }
    }
}