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
using System.Windows.Forms;
using System.IO;

namespace LabWork12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFolder = Directory.GetCurrentDirectory();

        public MainWindow()
        {
            InitializeComponent();
            txtFolderPath.Text = $"Текущая папка: {selectedFolder}";
        }

        private void btnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFolder = dialog.SelectedPath;
                txtFolderPath.Text = $"Текущая папка: {selectedFolder}";
            }
        }

        private void chkSizeFilter_Checked(object sender, RoutedEventArgs e)
        {
            txtMinSize.IsEnabled = true;
            txtMaxSize.IsEnabled = true;
        }

        private void chkSizeFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            txtMinSize.IsEnabled=false;
            txtMaxSize.IsEnabled=false;
            txtMinSize.Text = "";
            txtMaxSize.Text = "";
        }

        private void chkDateFilter_Checked(object sender, RoutedEventArgs e)
        {
            dtpDateFilter.IsEnabled = true;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            lstResults.Items.Clear();
            string searchPattern = $"*{txtFileName.Text}";
            SearchOption searchOption = rbIncludeSubfolders.IsChecked == true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            try
            {
                var files = Directory.GetFiles(selectedFolder, searchPattern, searchOption)
                    .Select(f => new FileInfo(f))
                    .Where(f => !chkSizeFilter.IsChecked.Value ||
                        (GetSizeInKB(f.Length) >= GetTextBoxValue(txtMinSize) &&
                        GetSizeInKB(f.Length) <= GetTextBoxValue(txtMaxSize)))
                    .Where(f => !chkDateFilter.IsChecked.Value || f.CreationTime >= dtpDateFilter.SelectedDate);

                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        lstResults.Items.Add("Файл найден: " + file.FullName);
                    }
                }
                else
                {
                    lstResults.Items.Add("Путём проверки файлов данный файл не был найден извините за это :( ");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK);
            }
        }

        private int GetSizeInKB(long bytes) => (int)(bytes / 1024);

        private int GetTextBoxValue(System.Windows.Controls.TextBox textBox)
        {
            return int.TryParse(textBox.Text, out int value) ? value : 0;
        }

        private void chkDateFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            dtpDateFilter.IsEnabled = false;
            dtpDateFilter.SelectedDate = null;
        }
    }
}