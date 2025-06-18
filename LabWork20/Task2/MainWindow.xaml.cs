using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer;
        private double _dx = 4, _dy = 4;
        private double _x, _y;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50)};
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
            //MoveClock();
        }

        private void UpdateTime()
        {
            ClockText.Text = DateTime.Now.ToString("HH:mm:ss");

            //ClockText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //ClockText.Arrange(new Rect(ClockText.DesiredSize));
        }

        private void PositionClockAtCenter()
        {
            //ClockText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //ClockText.Arrange(new Rect(ClockText.DesiredSize));
            //
            //_x = (this.Width - ClockText.ActualWidth);
            //_y = (this.Height - ClockText.ActualHeight);
            //Canvas.SetLeft(ClockText, _x);
            //Canvas.SetTop(ClockText, _y);
        }

        private void MoveClock()
        {
            //_x += _dx;
            //_y += _dy;
            //
            //double textWidth = ClockText.ActualWidth;
            //double textHeight = ClockText.ActualHeight;
            //
            //if (_x <= 0 || _x + textWidth >= this.Width)
            //    _dx = -_dx;
            //
            //if(_y <= 0 || _y + textHeight >= this.Height)
            //    _dy = -_dy;
            //
            //Canvas.SetLeft(ClockText, _x);
            //Canvas.SetTop(ClockText, _y);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTime();
            PositionClockAtCenter();
            _timer.Start();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}