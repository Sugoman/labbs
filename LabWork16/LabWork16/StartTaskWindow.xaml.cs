using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace LabWork16
{
    /// <summary>
    /// Логика взаимодействия для StartTaskWindow.xaml
    /// </summary>
    public partial class StartTaskWindow : Window
    {
        public StartTaskWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Исполняемые файлы (*.exe)|*.exe",
                Title = "Выберите файл"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                filePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void Ok_Button(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(filePathTextBox.Text))
            {
                try
                {
                    Process.Start(filePathTextBox.Text);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Укажите путь к файлу");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
