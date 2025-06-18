using System.Text;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace lab5
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("FireSharp installed successfully!");
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:\\dev\\K24GD03NhatTien\\serviceAccountKey.json")
            });
            Console.WriteLine("Firebase Admin SDK Da duoc khoi tao thanh cong!");

            await AddTestData();
            await ReadTestData();
            await UpdateTestData();
            await ReadTestData();
            await DeleteTestData();

        }

        public static async Task AddTestData()
        {
            var firebase = new FirebaseClient("https://tiennn22-f7a4c-default-rtdb.asia-southeast1.firebasedatabase.app/");

            var testData = new
            {
                Message = "Hello Firebase!",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM=dd HH:mm:ss")
            };

            await firebase.Child("test").PutAsync(testData);

            Console.WriteLine("Du lieu da duoc them vao Firebase");
        }
        public static async Task ReadTestData()
        {
            var firebase = new FirebaseClient("https://tiennn22-f7a4c-default-rtdb.asia-southeast1.firebasedatabase.app/");

            var testData = await firebase.Child("test").OnceSingleAsync<dynamic>();

            Console.WriteLine($"Message: {testData.Message}");
            Console.WriteLine($"Timestamp: {testData.Timestamp}");
        }
        public static async Task UpdateTestData()
        {
            var firebase = new FirebaseClient("https://tiennn22-f7a4c-default-rtdb.asia-southeast1.firebasedatabase.app/");

            var updatedData = new
            {
                Message = "Updated Massage!",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM=dd HH:mm:ss")
            };

            await firebase.Child("test").PatchAsync(updatedData);

            Console.WriteLine("Du lieu da duoc them vao Firebase");
        }
        public static async Task DeleteTestData()
        {
            var firebase = new FirebaseClient("https://tiennn22-f7a4c-default-rtdb.asia-southeast1.firebasedatabase.app/");

            await firebase.Child("test").DeleteAsync();

            Console.WriteLine("Du lieu da bi xoa khoi Firebase");
        }
    }
}
