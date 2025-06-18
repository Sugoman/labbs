using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpChat
{
    internal class Program
    {
        static IPAddress localAddress;
        static int localPort;
        static IPAddress remoteAddress;
        static int remotePort;
        static string? username;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите своё имя");

            username = Console.ReadLine();

            Console.WriteLine("Введите адрес для приёма сообщениий: ");

            localAddress = IPAddress.Parse(Console.ReadLine());

            Console.WriteLine("Введите порт для приёма сообщениий: ");

            localPort = int.Parse(Console.ReadLine());


            Console.WriteLine("Введите адрес для отправки сообщениий: ");

            remoteAddress = IPAddress.Parse(Console.ReadLine());

            Console.WriteLine("Введите порт для отправки сообщениий: ");

            remotePort = int.Parse(Console.ReadLine());


            ReciveMessageAsync();

            await SendMessageAsync();
        }

        static async Task ReciveMessageAsync()
        {
            byte[] data = new byte[1024];

            using UdpClient receiver = new UdpClient(new IPEndPoint(localAddress, localPort));

            while (true)
            {
                var result = await receiver.ReceiveAsync();
                data = result.Buffer;
                var message = Encoding.UTF8.GetString(data);
                Print(message);
            }
        }

        static async Task SendMessageAsync()
        {
            using UdpClient sender = new UdpClient();
            Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            while (true)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(message)) continue;

                message = $"{username}: {message}";

                byte[] data = Encoding.UTF8.GetBytes(message);

                await sender.SendAsync(data, new IPEndPoint(remoteAddress, remotePort));
            }
        }

        static void Print(string message)
        {
            if(OperatingSystem.IsWindows())
            {
                var position = Console.GetCursorPosition();

                int left = position.Left;
                int top = position.Top;

                Console.MoveBufferArea(0, top, left, 1, 0, top+1);
                Console.SetCursorPosition(0, top);
                Console.WriteLine(message);
                Console.SetCursorPosition(left, top+1);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
