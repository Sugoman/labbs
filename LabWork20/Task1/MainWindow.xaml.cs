using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _lastMousePosition;
        private bool _isFirstMove = true;

        public MainWindow()
        {
            InitializeComponent();
            _lastMousePosition = Mouse.GetPosition(this);
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(this);

            if(_isFirstMove || Math.Abs(currentMousePosition.X - _lastMousePosition.X) > 3 || Math.Abs(currentMousePosition.Y - _lastMousePosition.Y) > 3)
            {
                Close();
            }

            _isFirstMove = false;
            _lastMousePosition = currentMousePosition;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }
    }
}