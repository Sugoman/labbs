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

namespace RPS.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new GameClient();
        }

        private async void RockButton_Click(object sender, RoutedEventArgs e)
        {
            await PlayGame("камень");
        }

        private async void PaperButton_Click(object sender, RoutedEventArgs e)
        {
            await PlayGame("бумага");
        }

        private async void ScissorsButton_Click(object sender, RoutedEventArgs e)
        {
            await PlayGame("ножницы");
        }
        private async Task PlayGame(string move)
        {
            if(!client.IsConnected)
            {
                await client.Start("127.0.0.1", 5000);
            }

            

            await client.SendMoveAsync(move);

            resultTextBlock.Text = "ожидаем входа второго игрока";

            string result = await client.ReadResultAsync();

            resultTextBlock.Text = result;
        }

        private async void MuslimButton_Click(object sender, RoutedEventArgs e)
        {
            await PlayGame("муслим");
        }
    }
}