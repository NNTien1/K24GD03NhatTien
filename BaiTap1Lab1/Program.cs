using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Nhap x: ");
            int x = int.Parse(Console.ReadLine());

            Console.Write("Nhap y: ");
            int y = int.Parse(Console.ReadLine());

            double denominator = 2 * x - y;

            if (denominator == 0)
            {
                throw new DivideByZeroException("Mau so bang 0!");
            }

            double numerator = 3 * x + 2 * y;
            double fraction = numerator / denominator;

            if (fraction < 0)
            {
                throw new Exception("Gia tri duoi can bac hai khong đuoc am!");
            }

            double H = Math.Sqrt(fraction);

            Console.WriteLine($"H = {H}");

            using (StreamWriter writer = new StreamWriter("input.txt"))
            {
                writer.WriteLine($"x = {x}, y = {y}, H = {H}");
            }
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Loi: Đinh dang nhap khong hop le!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }
    }
}