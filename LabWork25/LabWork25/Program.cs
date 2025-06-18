using System.Net.Sockets;
using System.Net;

namespace LabWork25_1
{
    class Server
    {
        static void Main()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Ожидание клиента...");

            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключён.");

            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };

            while (true)
            {
                writer.WriteLine("Привет от сервера! " + DateTime.Now);
                Thread.Sleep(5000);
            }
        }
    }
}