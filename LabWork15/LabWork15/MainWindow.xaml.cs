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
using System.IO;

namespace LabWork15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.bmp;*.jpg;*.jpeg;*.png",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
                DisplayImage(_filePath);
            }
        }

        private void DisplayImage(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));
                DisplayedImage.Source = bitmap;

                Title = System.IO.Path.GetFileName(filePath);
                var fileInfo = new FileInfo(filePath);
                FileInfoText.Text = $"Размер: {fileInfo.Length / 1024} КБ | {bitmap.PixelWidth}x{bitmap.PixelHeight}px";

                ZoomSlider.Value = 1; // Сброс масштаба
                ImageScrollViewer.ScrollToHorizontalOffset(0);
                ImageScrollViewer.ScrollToVerticalOffset(0);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DisplayedImage != null || ImageScaleTransform == null)
                return;
            else
            {
                double scale = ZoomSlider.Value;
                ImageScaleTransform.ScaleX = scale;
                ImageScaleTransform.ScaleY = scale;
            }
        }
    }
}