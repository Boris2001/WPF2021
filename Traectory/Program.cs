using System;

namespace Traectory
{
    class Program
    {
		
static void Main(string[] args)
        {
			double x, y, xmax; //координаты
			double t, v, a; //время, скорость, угол
			Console.WriteLine("введите время");
			t = Convert.ToDouble( Console.ReadLine());
			Console.WriteLine("введите скорость");
			v = Convert.ToDouble(Console.ReadLine());
			Console.WriteLine("введите угол");
			a = Convert.ToDouble(Console.ReadLine());
			x = v * Math.Cos(a * (Math.PI / 180)) * t;
			y = v * Math.Sin(a * (Math.PI / 180)) * t - 9.8 * t*t / 2;
			xmax = v*v * Math.Sin(2 * a * (Math.PI / 180)) / 9.8;
			if (x > xmax)
			{
				Console.WriteLine( "X равен: " + xmax );
				Console.WriteLine("Y равен:0");
			}
			else
			{
				Console.WriteLine("X равен: " + x );
				Console.WriteLine("Y равен: " + y);
			}
        }
    }
}
