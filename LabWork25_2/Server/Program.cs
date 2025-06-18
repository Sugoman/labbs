using System.Net;
using System.Net.Sockets;

class Server
{
    static List<StreamWriter> clients = new List<StreamWriter>();

    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 5001);
        listener.Start();
        Console.WriteLine("Сервер запущен.");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Клиент подключился.");
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            StreamReader reader = new StreamReader(client.GetStream());

            clients.Add(writer);

            new Thread(() =>
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");
                    string message = $"{line} ({timestamp})";
                    Console.WriteLine(message);
                    foreach (var w in clients)
                    {
                        try { w.WriteLine(message) ; } catch { }
                    }
                }
            }).Start();
        }
    }
}