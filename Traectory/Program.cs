using System;
using System.IO;
using System.Collections.Generic;

namespace Trajectory
{
    /*//Программу брал у Андрея Речкалова
using System;
using System.IO;

namespace Trajectory
{
    class Trajectory1
    {
        public double speed, time, x, y, angle, steps;
        string path;
        public Trajectory1()
        {
            Console.WriteLine("Введите название файла, у которого установлено расширение csv");
            path = @"D:\visual_projects\sharp\Trajectory_1\" + Console.ReadLine() + ".csv";
            speed = reading_from_file(path, 0);
            angle = Math.PI * reading_from_file(path, 1) / 180;
            time = reading_from_file(path, 2);
            steps = reading_from_file(path, 3);
        }
        public double reading_from_file(string path, int i)
        {
            string line = null;
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                for (int j = 0; j <= i; ++j)
                {
                    if ((line = sr.ReadLine()) == null)
                    {
                        break;
                    }
                }
            }
            if (line != null)
                return Convert.ToDouble(line);
            else return 0;
        }
        public void out_put(double time)
        {
            x = speed * Math.Cos(angle) * time;
            y = speed * Math.Sin(angle) * time - 4.9 * time * time;
            if (y <= 0)
                y = 0;
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.Write("\n" + "x(");
                sw.Write("{0:0.##}", time);
                sw.Write(") = ");
                sw.Write("{0:0.##}", x);
                sw.Write("\t" + "y(");
                sw.Write("{0:0.##}", time);
                sw.Write(") = ");
                sw.Write("{0:0.##}", y);
            }
        }
        public void cycle(double time, double steps)
        {
            for (double i = 0; i < time; i += steps)
            {
                out_put(i);
            }
            out_put(time);
        }
    }
    class Trajectory
    {
        public static void Main(string[] args)
        {
            Trajectory1 trajectory = new Trajectory1();
            trajectory.cycle(trajectory.time, trajectory.steps);
        }
    }
}
*/


    class Bodyflight
    {
        struct Coords
        {
            public Coords(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double X { get; set; }
            public double Y { get; set; }
        }
        List<Coords> Coordinates = new List<Coords>();
        double g = 9.81, steps = 0.005, starting_speed, angle, drag_coefficient = 0.47, medium_density = 1.2754, body_radius, resistance_force;
        private void Input()
        {
            Console.WriteLine("Введите начальную скорость");
            starting_speed = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Введите угол в градусах");
            angle = Math.PI * Convert.ToDouble(Console.ReadLine()) / 180;
            Console.WriteLine("Введите радиус тела");
            body_radius = Convert.ToDouble(Console.ReadLine());

            Calculation_the_resistance_force(starting_speed);

        }

        void Calculation_the_resistance_force(double speed)
        {
            resistance_force = drag_coefficient * medium_density * speed * speed / 2
                    * Math.PI * body_radius * body_radius;
        }
        private void CountingcCordinates()
        {
            double speed;
            double speedx = starting_speed * Math.Cos(angle);
            double speedy = starting_speed * Math.Sin(angle);
            Coords coords = new Coords(0, 0);
            Coordinates.Add(coords);
            while (coords.Y >= 0)
            {
                coords.X = coords.X + steps * speedx;
                speedx = speedx - steps * resistance_force * speedx;
                coords.Y = coords.Y + steps * speedy;
                speedy = speedy - steps * (g + resistance_force * speedy);
                if (coords.Y <= 0)
                    break;
                Coordinates.Add(coords);
                speed = Math.Sqrt(speedx * speedx + speedy * speedy);
                Calculation_the_resistance_force(speed);
            }
            coords.Y = 0;
            Coordinates.Add(coords);
        }
        public void Output()
        {
            string path = @"D:\visual_projects\sharp\Trajectory_1\" + Console.ReadLine() + ".csv";
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                Input();
                CountingcCordinates();
                for (int i = 1; i < Coordinates.Count + 1; ++i)
                {
                    sw.Write("x(");
                    sw.Write("{0:0.###}", steps * Convert.ToDouble(i));
                    sw.Write(") = ");
                    sw.Write("{0:0.###}", Coordinates[i - 1].X);
                    sw.Write("\t" + "y(");
                    sw.Write("{0:0.###}", steps * Convert.ToDouble(i));
                    sw.Write(") = ");
                    sw.Write("{0:0.###}", Coordinates[i - 1].Y);
                }
            }
        }
        public Bodyflight() { }
    }

    class Trajectory
    {
        public static void Main(string[] args)
        {
            Bodyflight trajectory = new Bodyflight();
            trajectory.Output();
        }
    }
}
