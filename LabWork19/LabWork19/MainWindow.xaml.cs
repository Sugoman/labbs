using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Windows;
using Windows.Storage.Pickers;

namespace LabWork19
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentArchivePath;
        private ObservableCollection<ZipEntryInfo> archiveEntries = new();
        public MainWindow()
        {
            InitializeComponent();
            ArchiveContents.ItemsSource = archiveEntries;
        }

        private void CreateArchiveFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { Title = "Выберите файл" };
            if(openFileDialog.ShowDialog() == true)
            {
                SaveFileDialog saveFileDialog = new() { Filter = "ZIP Archive|*zip", Title="Сохранить архив"};
                if(saveFileDialog.ShowDialog() == true)
                {
                    using FileStream fs = new(saveFileDialog.FileName, FileMode.Create);
                    using ZipArchive archive = new(fs, ZipArchiveMode.Create);
                    archive.CreateEntryFromFile(openFileDialog.FileName, System.IO.Path.GetFileName(openFileDialog.FileName));
                    MessageBox.Show("Файл добавлен в архив!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    currentArchivePath = saveFileDialog.FileName;
                    LoadArchiveContents();
                }
            }
        }

        private void CreateArchiveFromFolder_Click(object sender, RoutedEventArgs e)
        {
            string? folderPath = SelectFolder();
            if(!string.IsNullOrWhiteSpace(folderPath))
            {
                SaveFileDialog saveFileDialog = new() { Filter = "ZIP Archive|*zip", Title = "Сохранить архив" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    ZipFile.CreateFromDirectory(folderPath, saveFileDialog.FileName);
                    MessageBox.Show("Папка заархивирована!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    currentArchivePath = saveFileDialog.FileName;
                    LoadArchiveContents();
                }
            }
        }

        private void OpenArchive_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { Filter = "ZIP Archive|*zip", Title = "Сохранить архив" };
            if(openFileDialog.ShowDialog() == true)
            {
                currentArchivePath = openFileDialog.FileName;
                LoadArchiveContents();
            }
        }

        private void LoadArchiveContents()
        {
            if (string.IsNullOrWhiteSpace(currentArchivePath)) return;
            archiveEntries.Clear();

            using ZipArchive archive = ZipFile.OpenRead(currentArchivePath);
            foreach (var entry in archive.Entries)
            {
                archiveEntries.Add(new ZipEntryInfo { Name = entry.FullName, ComprossedSize = $"{entry.CompressedLength / 1024} KB" });
            }
        }

        private void AddToArchive_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(currentArchivePath))
            {
                MessageBox.Show("Сначала откройте архив!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OpenFileDialog openFileDialog = new() { Multiselect = true, Title = "Выберите файлы" };
            if (openFileDialog.ShowDialog() == true)
            {
                using FileStream fs = new(currentArchivePath, FileMode.Open);
                using ZipArchive archive = new(fs, ZipArchiveMode.Update);


                foreach (var file in openFileDialog.FileNames)
                {
                    archive.CreateEntryFromFile(file, System.IO.Path.GetFileName(file));
                }
            }
            LoadArchiveContents();
        }

        private void ExtractAll_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(currentArchivePath))
            {
                MessageBox.Show("Сначала откройте архив!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string? folderPath = SelectFolder();
            if(!string.IsNullOrWhiteSpace(folderPath))
            {
                ZipFile.ExtractToDirectory(currentArchivePath, folderPath);
                MessageBox.Show("Файлы извлечены!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information );
            }
        }

        private void ExtractSelectedFile_Click(object sender, RoutedEventArgs e)
        {
            if (ArchiveContents.SelectedItems is not ZipEntryInfo selectedFile)
            {
                MessageBox.Show("Выберите файл из списка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new() { Title = "Сохранить файл", FileName = selectedFile.Name };
            if (saveFileDialog.ShowDialog() == true)
            {
                using ZipArchive archive = ZipFile.OpenRead(currentArchivePath);
                var entry = archive.Entries.FirstOrDefault(e => e.FullName == selectedFile.Name);
                entry?.ExtractToFile(saveFileDialog.FileName, true);
                MessageBox.Show("Файл извлечён!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private string? SelectFolder()
        {
            var picker = new FolderPicker();
            return SelectFolder(); 
        }
    }
}