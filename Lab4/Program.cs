using System.Collections;

namespace Lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hashtable danhsach = new Hashtable();
            //nhap mat hang
            while (true)
            {
                Console.Write("Nhap ma mat hang: ");
                int ma = int.Parse(Console.ReadLine());
                Console.Write("Nhap ten mat hang: ");
                string ten = Console.ReadLine();
                Console.Write("Nhap so luong: ");
                int sl = int.Parse(Console.ReadLine());
                Console.Write("Nhap đon gia: ");
                float gia = float.Parse(Console.ReadLine());

                MatHang mh = new MatHang(ma, ten, sl, gia);
                ThemMatHang(danhsach, mh);

                Console.Write("Nhan Enter đe tiep tuc, hoac go 'k' đe ket thuc: ");
                if (Console.ReadLine().ToLower() == "k")
                    break;
            }
            //xuat mat hang
            Console.WriteLine("\nDanh sach mat hang");
            foreach (MatHang mh in danhsach.Values)
            {
                mh.Xuat();

            }

            // Tìm và xóa mặt hàng
            Console.Write("\nNhap ma mat hang can tim va xoa: ");
            int maTim = int.Parse(Console.ReadLine());

            if (TimMatHang(danhsach, maTim))
            {
                MatHang mhCanXoa = (MatHang)danhsach[maTim];
                Console.WriteLine("\nMat hang can tim:");
                mhCanXoa.Xuat();

                XoaMatHang(danhsach, maTim);
                Console.WriteLine("\nDa xoa mat hang. Danh sach sau khi xoa: ");
                foreach (MatHang mh in danhsach.Values)
                {
                    mh.Xuat();
                }
            }
            else
            {
                Console.WriteLine("Khong tim thay mat hang");
            }
            Console.ReadLine();
        }

        struct MatHang
        {
            public int MaMH;
            public string TenMH;
            public int SoLuong;
            public float DonGia;

            public MatHang(int MaMH, string TenMH, int SoLuong, float DonGia)
            {
                this.MaMH = MaMH;
                this.TenMH = TenMH;
                this.SoLuong = SoLuong;
                this.DonGia = DonGia;
            }

            public float ThanhTien()
            {
                return SoLuong * DonGia;
            }

            public void Xuat()
            {
                Console.WriteLine($"Ma: {MaMH}, Ten: {TenMH}, SL: {SoLuong}, Đon gia: {DonGia:N0}, Thanh tien: {ThanhTien():N0}");
            }
        }

        static void ThemMatHang(Hashtable danhsach, MatHang m)
        {
            if (!danhsach.ContainsKey(m.MaMH))
                danhsach.Add(m.MaMH, m);
            else
                Console.WriteLine("Ma mat hang da ton tai!");
        }

        static bool TimMatHang(Hashtable danhsach, int MaMH)
        {
            return danhsach.ContainsKey(MaMH);
        }

        static void XoaMatHang(Hashtable danhsach, int MaMH)
        {
            if (TimMatHang(danhsach, MaMH))
                danhsach.Remove(MaMH);
        }
    }
}

