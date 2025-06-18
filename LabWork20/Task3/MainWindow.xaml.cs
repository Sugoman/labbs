using System.IO;
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
using System.Windows.Threading;

namespace Task3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] _imageFiles;
        private int _currentImageIndex = 0;
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            string imageFolder = @"C:\Temp\ispp21\Картинки";
            if(Directory.Exists(imageFolder))
                _imageFiles = Directory.GetFiles(imageFolder, "*.*").Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToArray();

            if (_imageFiles.Length > 0)
                ShowNextImage();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10)};
            _timer.Tick += (s, e) => ShowNextImage();
            _timer.Start();
        }

        private void ShowNextImage()
        {
            if (_imageFiles.Length == 0) return;
            PhotoDisplay.Source = new BitmapImage(new Uri(_imageFiles[_currentImageIndex]));
            _currentImageIndex = (_currentImageIndex + 1) % _imageFiles.Length;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }
    }
}