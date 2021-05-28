using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Trajectory_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private void OnTimer(Object sender, EventArgs args) 
        {
            if (coords_y >= 0)
            {
                coords_x = coords_x + steps * speedx;
                graph_x = actual_width + 1000 * coords_x;
                speedx = speedx - steps * resistance_force * speedx;
                coords_y = coords_y + steps * speedy;
                graph_y = actual_height - 1000 * coords_y;
                speedy = speedy - steps * (g + resistance_force * speedy);
                if (coords_y <= 0 && speed != 0)
                {
                    graph_y = actual_height;
                    polyline.Points.Add(new Point(graph_x, graph_y));
                    Canvas.SetLeft(ellipse, polyline.Points.Last().X - ellipse.Width / 2.0);
                    Canvas.SetTop(ellipse, polyline.Points.Last().Y - ellipse.Height / 2.0);
                    Distance_TextBlock.Text = Convert.ToString(coords_x);
                    timer.Stop();
                } 
                else
                {
                    polyline.Points.Add(new Point(graph_x, graph_y));
                    Canvas.SetLeft(ellipse, polyline.Points.Last().X - ellipse.Width / 2.0);
                    Canvas.SetTop(ellipse, polyline.Points.Last().Y - ellipse.Height / 2.0);
                    if (polyline.Points.Count == 1)
                    {
                        ellipse.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if (speed == 0)
                    {
                        speedx = starting_speed * Math.Cos(angle);
                        speedy = starting_speed * Math.Sin(angle);
                    }
                    speed = Math.Sqrt(speedx * speedx + speedy * speedy);
                    Calculation_the_resistance_force(speed);
                }

            }
            else
            {
                graph_y = actual_height;
                polyline.Points.Add(new Point(graph_x, graph_y));
                Canvas.SetLeft(ellipse, polyline.Points.Last().X - ellipse.Width / 2.0);
                Canvas.SetTop(ellipse, polyline.Points.Last().Y - ellipse.Height / 2.0);
                Distance_TextBlock.Text = Convert.ToString(coords_x);
                timer.Stop();
            }
            
        }

        double actual_width, actual_height;

        double speed, speedx, speedy;

        double coords_x, coords_y, graph_x, graph_y;
        double distance, time, max_height = 0;
        private static readonly Regex _regex = new Regex("[^0-9,]");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
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

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Speed_TextBox.Text != "" && Angle_TextBox.Text != "" && Radius_TextBox.Text != "")
                Output();
            else
                MessageBox.Show("Не введены начальные данные", "Trajectory_2", MessageBoxButton.OK, MessageBoxImage.Error);
        }

       

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            Speed_TextBox.Text = "";
            Angle_TextBox.Text = "";
            Radius_TextBox.Text = "";
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        public void Input()
        {
            starting_speed = Convert.ToDouble(Speed_TextBox.Text);
            angle = Math.PI * Convert.ToDouble(Angle_TextBox.Text) / 180;
            body_radius = Convert.ToDouble(Radius_TextBox.Text);
            Calculation_the_resistance_force(starting_speed);
        }

        private void clear()
        {
            speed = speedx = speedy = 0;
            coords_x = coords_y = 0;
            ellipse.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        private void Output()
        {
            Input();
            polyline.Points.Clear();
            actual_width = canvas.ActualWidth / 2;
            actual_height = canvas.ActualHeight / 2;
            clear();
            timer.Start();
            //CountingcCordinates();
            Distance_TextBlock.Text = Convert.ToString(distance);
            Time_TextBlock.Text = Convert.ToString(time);
            MaxHeight_Textblock.Text = Convert.ToString(max_height);
        }

        private void Calculation_the_resistance_force(double speed)
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
                if (coords.Y > max_height)
                    max_height = coords.Y;
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
            distance = coords.X;
            time = steps * Coordinates.Count;
        }

        /*private bool Compare_Height()
        {

        }*/
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(OnTimer);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }
    }
}
