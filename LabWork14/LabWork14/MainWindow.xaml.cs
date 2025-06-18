using System.Diagnostics;
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

namespace LabWork14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class FileInfoModel
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public long FileSize { get; set; } // Размер в байтах
            public DateTime LastModified { get; set; } // Дата изменения
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtDirectory.Text = dialog.SelectedPath;
                }
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string directory = txtDirectory.Text;
            if (!Directory.Exists(directory))
            {
                System.Windows.MessageBox.Show("Каталог не найден.");
                return;
            }

            var files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories)
                                 .Select(file => new FileInfo(file))
                                 .Select(file => new FileInfoModel
                                 {
                                     FileName = file.Name,
                                     FilePath = file.FullName,
                                     FileSize = file.Length / 1024, // KB
                                     LastModified = file.LastWriteTime
                                 }).ToList();

            var duplicates = FindDuplicates(files);
            dgResults.ItemsSource = duplicates;
        }

        // Метод поиска дубликатов
        private List<FileInfoModel> FindDuplicates(List<FileInfoModel> files)
        {
            var groupedFiles = files.GroupBy(f => new
            {
                Name = chkName.IsChecked == true ? f.FileName : null,
                Size = chkSize.IsChecked == true ? f.FileSize : (long?)null,
                Date = chkDate.IsChecked == true ? f.LastModified.Date : (DateTime?)null
            })

            .Where(g => g.Count() > 1)
                               .SelectMany(g => g)
                               .ToList();
            return groupedFiles;
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            string filePath = (sender as System.Windows.Controls.Button)?.Tag as string;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true // Открывает файл в ассоциированной программе
                    });
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Файл не найден.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string filePath = (sender as System.Windows.Controls.Button)?.Tag as string;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    if (System.Windows.MessageBox.Show("Удалить файл?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        File.SetAttributes(filePath, FileAttributes.Normal); // Убираем атрибут "Только чтение"
                        File.Delete(filePath);

                        System.Windows.MessageBox.Show("Файл удалён.");
                        BtnSearch_Click(null, null); // Обновить список файлов
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при удалении файла: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Файл не найден.");
            }
        }

    }
}