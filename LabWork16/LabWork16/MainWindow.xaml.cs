using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace LabWork16
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ProcessInfo> processes = new();
        private ObservableCollection<AppInfo> applications = new();
        public MainWindow()
        {
            InitializeComponent();
            LoadProcesses();
            LoadApplications();
        }

        public void LoadProcesses()
        {
            processes.Clear();
            foreach (var process in Process.GetProcesses())
            {
                processes.Add(new ProcessInfo
                {
                    ProcessName = process.ProcessName,
                    Id = process.Id,
                    Memory = process.WorkingSet64 / 1024 / 1024
                });
            }
            processListView.ItemsSource = processes;
            statusTextBlock.Text = $"Процессов {processes.Count}";
        }

        public void LoadApplications()
        {
            applications.Clear();
            foreach (var process in Process.GetProcesses().Where(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle)))
            {
                applications.Add(new AppInfo
                {
                    Title = process.MainWindowTitle,
                    //StartTime = process.StartTime.ToString("HH:mm:ss")
                });
            }
        }

        public void StartNewTask_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartTaskWindow();
            startWindow.ShowDialog();
            LoadProcesses();
        }

        public void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadApplications();
            LoadProcesses();
        }

        public void EndTask_Click(object sender, RoutedEventArgs e)
        {
            if (processListView.SelectedItem is ProcessInfo selectedProcess)
            {
                try
                {
                    Process.GetProcessById(selectedProcess.Id).Kill();
                    Process.GetProcessById(selectedProcess.Id).WaitForExit();
                    LoadApplications();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        public void KillProccessTree_Click(object sender, RoutedEventArgs e)
        {
            if (processListView.SelectedItem is ProcessInfo selectedProcess)
            {
                try
                {
                    var process = Process.GetProcessById(selectedProcess.Id);
                    process.Kill(true);
                    LoadProcesses();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}