using System.Net.Sockets;

class ConsoleClient
{
    static void Main()
    {
        TcpClient client = new TcpClient("127.0.0.1", 5001);
        StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
        StreamReader reader = new StreamReader(client.GetStream());

        Console.WriteLine("Введите имя: ");
        string name = Console.ReadLine();

        new Thread(() =>
        {
            while (true)
                Console.WriteLine(reader.ReadLine);
        }).Start();

        while (true)
        {
            string message = Console.ReadLine();
            writer.WriteLine($"{name}: {message}");
        }
    }
}