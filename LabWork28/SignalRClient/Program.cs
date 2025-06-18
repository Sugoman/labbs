using System.Text;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Подключение к чату...");

        try
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7039/chat", options =>
                {
                    // Отключаем проверку SSL для разработки
                    options.HttpMessageHandlerFactory = handler =>
                        new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                (message, cert, chain, errors) => true
                        };
                })
                .Build();

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            await connection.StartAsync();
            Console.WriteLine("Подключено!");

            Console.Write("Введите ваше имя: ");
            var userName = Console.ReadLine();

            Console.Write("Введите название комнаты: ");
            var roomName = Console.ReadLine();

            await connection.InvokeAsync("JoinRoom", roomName, userName);

            while (true)
            {
                var message = Console.ReadLine();
                if (!string.IsNullOrEmpty(message))
                {
                    await connection.InvokeAsync("SendMessage", message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}