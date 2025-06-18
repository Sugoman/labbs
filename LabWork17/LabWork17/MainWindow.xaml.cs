using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace LabWork17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Folder> RootFolders { get; set; } = new ObservableCollection<Folder>();
        public MainWindow()
        {
            InitializeComponent();
            LoadDrivers();
            FolderTree.ItemsSource = RootFolders;
        }

        private void LoadDrivers()
        {
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady))
            {
                RootFolders.Add(new Folder(drive.Name));
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            { 
                string path = PathTextBox.Text;
                if (Directory.Exists(path))
                {
                    var folder = new Folder(path);
                    LoadFolderContents(folder);
                }
                else
                {
                    MessageBox.Show("Папка не найдена!");
                }
            }
        }

        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Folder folder)
            {
                PathTextBox.Text = folder.Path;
                LoadFolderContents(folder);
            }
        }

        private void LoadFolderContents(Folder folder)
        {
            folder.SubFolders.Clear();
            folder.Files.Clear();

            try
            {
                foreach (var dir in Directory.GetDirectories(folder.Path))
                    folder.SubFolders.Add(new Folder(dir));

                foreach (var file in Directory.GetFiles(folder.Path))
                    folder.Files.Add(new FileItem(file));

                FileList.ItemsSource = folder.Files;
            }
            catch { }
        }

        private void CopyFiles_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = FileList.SelectedItems.Cast<FileItem>().Select(f => f.Path).ToArray();
            if(selectedFiles.Length > 0)
            {
                StringCollection fileCollection = new StringCollection();
                fileCollection.AddRange(selectedFiles);

                Clipboard.SetFileDropList(fileCollection);
            }
        }

        private void PasteFiles_Click(object sender, RoutedEventArgs e)
        {
            if(!Directory.Exists(PathTextBox.Text)) return;

            var files = Clipboard.GetFileDropList();
            foreach (string file in files)
            {
                string dest = System.IO.Path.Combine(PathTextBox.Text, file);
                File.Copy(file, dest, true);
            }
        }
    }
}