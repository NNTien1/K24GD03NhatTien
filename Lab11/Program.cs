using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database;
using Firebase.Database.Query;

namespace Lab11
{
    public class Player
    {
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Coins { get; set; }
        public int VipLevel { get; set; }
        public string Region { get; set; }
        public DateTime LastLogin { get; set; }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/main/simple_players.json";
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(url);
            List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);

            var firebase = new FirebaseClient("https://nhattien1231-8c327-default-rtdb.asia-southeast1.firebasedatabase.app/");

            // ======= BAI 1 =======
            var richPlayers = players
                .Where(p => p.Gold > 1000 && p.Coins > 100)
                .OrderByDescending(p => p.Gold)
                .Select(p => new { p.Name, p.Gold, p.Coins })
                .ToList();

            Console.WriteLine("=== Nguoi choi giau ===");
            foreach (var p in richPlayers)
                Console.WriteLine($"Name: {p.Name}, Gold: {p.Gold}, Coins: {p.Coins}");

            await firebase.Child("RichPlayers").PutAsync(richPlayers);

            // ======= BAI 2 =======
            Console.WriteLine("\n=== Thong ke VIP ===");

            int vipCount = players.Count(p => p.VipLevel > 0);
            Console.WriteLine($"\nTong so nguoi choi VIP: {vipCount}");

            var vipByRegion = players
                .Where(p => p.VipLevel > 0)
                .GroupBy(p => p.Region)
                .Select(g => new { Region = g.Key, Count = g.Count() });

            Console.WriteLine("\nSo luong nguoi choi VIP theo Region:");
            foreach (var g in vipByRegion)
                Console.WriteLine($"Region: {g.Region}, So nguoi choi Vip: {g.Count}");

            DateTime now = new DateTime(2025, 06, 30, 0, 0, 0);
            var recentVip = players
                .Where(p => p.VipLevel > 0 && (now - p.LastLogin).TotalDays <= 2)
                .Select(p => new { p.Name, p.VipLevel, p.LastLogin })
                .ToList();

            Console.WriteLine("\nNguoi choi VIP moi dang nhap gan day:");
            foreach (var p in recentVip)
                Console.WriteLine($"Name: {p.Name}, VIP: {p.VipLevel}, LastLogin: {p.LastLogin}");

            await firebase.Child("RecentVipPlayer").PutAsync(recentVip);
        }
    }
}