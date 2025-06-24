using System;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Lab6_1Menu
{
    class Program
    {
        static FirebaseClient firebase;
        static SinhVien sinhVien;
        static string mssv;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("C:\\dev\\K24GD03NhatTien\\serviceAccountKey.json")
            });

            firebase = new FirebaseClient("https://nhattien1231-8c327-default-rtdb.asia-southeast1.firebasedatabase.app/");

            while (true)
            {
                Console.WriteLine("\n=== MENU ===");
                Console.WriteLine("1. Nhập dữ liệu sinh viên");
                Console.WriteLine("2. Ghi dữ liệu vào Firebase");
                Console.WriteLine("3. Lấy dữ liệu từ Firebase");
                Console.WriteLine("4. Cập nhật dữ liệu sinh viên");
                Console.WriteLine("5. Xóa dữ liệu sinh viên");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await NhapSinhVien(); break;
                    case "2": await GhiSinhVien(); break;
                    case "3": await LaySinhVien(); break;
                    case "4": await CapNhatSinhVien(); break;
                    case "5": await XoaSinhVien(); break;
                    case "0": return;
                    default: Console.WriteLine("Lựa chọn không hợp lệ."); break;
                }
            }
        }

        public class SinhVien
        {
            public string HoTen { get; set; }
            public string MSSV { get; set; }
            public double Diem { get; set; }
            public string Lop { get; set; }
        }

        static async Task NhapSinhVien()
        {
            Console.Write("Họ tên: ");
            string hoTen = Console.ReadLine();
            Console.Write("MSSV: ");
            mssv = Console.ReadLine();
            Console.Write("Điểm: ");
            double diem = double.Parse(Console.ReadLine());
            Console.Write("Lớp: ");
            string lop = Console.ReadLine();

            sinhVien = new SinhVien
            {
                HoTen = hoTen,
                MSSV = mssv,
                Diem = diem,
                Lop = lop
            };
        }

        static async Task GhiSinhVien()
        {
            if (sinhVien == null)
            {
                Console.WriteLine("Chưa có dữ liệu sinh viên. Hãy nhập trước!");
                return;
            }

            await firebase.Child("SinhVien").Child(mssv).PutAsync(sinhVien);
            Console.WriteLine("Đã ghi dữ liệu vào Firebase.");
        }

        static async Task LaySinhVien()
        {
            Console.Write("Nhập MSSV cần đọc: ");
            string maSo = Console.ReadLine();

            var sv = await firebase.Child("SinhVien").Child(maSo).OnceSingleAsync<SinhVien>();

            if (sv != null)
            {
                Console.WriteLine($"Họ tên: {sv.HoTen}, MSSV: {sv.MSSV}, Điểm: {sv.Diem}, Lớp: {sv.Lop}");
            }
            else
            {
                Console.WriteLine("Không tìm thấy sinh viên với MSSV này.");
            }
        }

        static async Task CapNhatSinhVien()
        {
            Console.Write("Nhập MSSV cần cập nhật: ");
            string maSo = Console.ReadLine();
            Console.Write("Điểm mới: ");
            double diemMoi = double.Parse(Console.ReadLine());

            var update = new { Diem = diemMoi };
            await firebase.Child("SinhVien").Child(maSo).PatchAsync(update);
            Console.WriteLine("Đã cập nhật điểm.");
        }

        static async Task XoaSinhVien()
        {
            Console.Write("Nhập MSSV cần xóa: ");
            string maSo = Console.ReadLine();
            await firebase.Child("SinhVien").Child(maSo).DeleteAsync();
            Console.WriteLine("Đã xóa sinh viên khỏi Firebase.");
        }
    }
}
