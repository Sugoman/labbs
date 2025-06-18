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
using System.Threading.Tasks;
using System.Windows.Input;
using Path = System.IO.Path;


namespace LabWork18
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDriversInfo();
        }

        private void LoadDriversInfo()
        {
            var drivers = DriveInfo.GetDrives()
            .Where(d => d.IsReady)
            .Select(d => new
            {
                Name = d.Name,
                DriveType = d.DriveType,
                TotalSize = FormatSize(d.TotalSize),
                UsedPercentage = $"{(d.TotalSize - d.AvailableFreeSpace) * 100 / d.TotalSize}%",
                FreeSpace = FormatSize(d.AvailableFreeSpace)
            });
            DiskListView.ItemsSource = drivers;
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private async void PathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string folderPath = PathTextBox.Text;
                if (Directory.Exists(folderPath))
                {
                    await AnalyzeFolderAsync(folderPath);
                }
                else
                {
                    MessageBox.Show("Папка не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error );
                }
            }
        }

        private async Task AnalyzeFolderAsync(string folderPath)
        {
            await Task.Run(() =>
            {
                var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                var folders = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);

                long totalSize = files.Sum(f => new FileInfo(f).Length);
                var drive = new DriveInfo(Path.GetPathRoot(folderPath));
                double usedPercentage = (double)totalSize / drive.TotalSize * 100;

                Dispatcher.Invoke(() =>
                {
                    FilesCountText.Text = $"Файлов: {files.Length}";
                    FoldersCountText.Text = $"Папок: {folders.Length}";
                    FolderSizeText.Text = $"Размер: {FormatSize(totalSize)}";
                    UsedPercentageText.Text = $"Занято на диске: {usedPercentage:0.##}%";
                });

                var topFiles = files.Select(f => new FileInfo(f))
                .OrderByDescending(f => f.Length)
                .Take(10)
                .Select(f => new
                {
                    Name = f.Name,
                    Size = FormatSize(f.Length),
                    Path = f.FullName,
                    Modified = f.LastWriteTime
                })
                .ToList();

                Dispatcher.Invoke(() => TopFilesView.ItemsSource = topFiles);
            });
        }
    }
}