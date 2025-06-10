namespace Lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a, b;
            Console.WriteLine("Nhap so a: ");
            a = double.Parse(Console.ReadLine());
            Console.WriteLine("Nhap so b: ");
            b = double.Parse(Console.ReadLine());
            double Thuong = a / b;
            double Tong = a + b;
            double Hieu = a - b;
            double Tich = a * b;
            Console.WriteLine("Thuong la: " + Thuong);
            Console.WriteLine("Tong la: " + Tong);
            Console.WriteLine("Hieu la: " + Hieu);
            Console.WriteLine("Tich la: " + Tich);
        }
    }
}
