using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database;
using Firebase.Database.Query;

namespace LAB12_FINAL
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLogin { get; set; }
        public int Level { get; set; }
        public int VipLevel { get; set; }
        public int Gold { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var firebase = new FirebaseClient("https://nhattien1231-8c327-default-rtdb.asia-southeast1.firebasedatabase.app/");
            DateTime now = new DateTime(2025, 06, 30, 0, 0, 0, DateTimeKind.Utc);

            var players = await GetPlayersFromJsonAsync(
                "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/refs/heads/main/lab12_players.json");

            // Bai 1.1 - Inactive Players
            var inactive = players.Where(p => !p.IsActive || (now - p.LastLogin).TotalDays > 5).ToList();
            Console.WriteLine("\n-- Inactive Players --");
            inactive.ForEach(p => Console.WriteLine($"Name: {p.Name}, Active: {p.IsActive}, LastLogin: {p.LastLogin}"));
            await firebase.Child("Inactive_players").PutAsync(inactive);

            // Bai 1.2 - Low Level Players
            var lowLevel = players.Where(p => p.Level < 10).ToList();
            Console.WriteLine("\n-- Low Level Players --");
            lowLevel.ForEach(p => Console.WriteLine($"Name: {p.Name}, Level: {p.Level}, Gold: {p.Gold}"));
            await firebase.Child("Low_level_players").PutAsync(lowLevel);

            // Bai 2 - Top 3 VIP Award
            var top3 = players.Where(p => p.VipLevel > 0)
                              .OrderByDescending(p => p.Level)
                              .Take(3)
                              .ToList();

            int[] rewards = { 2000, 1500, 1000 };
            Console.WriteLine("\n-- Top 3 VIP Players --");
            for (int i = 0; i < top3.Count; i++)
            {
                var p = top3[i];
                Console.WriteLine($"Name: {p.Name}, Vip: {p.VipLevel}, Level: {p.Level}, Gold: {p.Gold}, Reward: {rewards[i]}");
            }

            await firebase.Child("Top3_vip_awards").PutAsync(top3);
        }

        static async Task<List<Player>> GetPlayersFromJsonAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Player>>(json);
            }
        }
    }
}
