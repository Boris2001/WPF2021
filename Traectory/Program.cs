//Программу брал у Андрея Речкалова
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
