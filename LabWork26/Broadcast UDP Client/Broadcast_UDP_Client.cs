using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

const int port = 12345;
const int broadcastInterval = 1000;
string playerName = Environment.UserName;

Random random = new Random();
var position = new { X = random.Next(0, 100), Y = random.Next(0, 100) };

using var udpClient = new UdpClient(port);
udpClient.EnableBroadcast = true;

var broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, port);

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) => cts.Cancel();

var sendTask = Task.Run(async () =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        position = new { X = random.Next(0, 100), Y = random.Next(0, 100) };

        var message = new
        {
            PlayerName = playerName,
            Time = DateTime.Now,
            Position = position
        };

        string json = JsonSerializer.Serialize(message);
        byte[] data = Encoding.UTF8.GetBytes(json);

        await udpClient.SendAsync(data, data.Length, broadcastEndPoint);
        Console.WriteLine($"Отправлено: {json}");

        await Task.Delay(broadcastInterval, cts.Token);
    }
});

var receiveTask = Task.Run(async () =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        try
        {
            var result = await udpClient.ReceiveAsync(cts.Token);
            string receivedJson = Encoding.UTF8.GetString(result.Buffer);

            var receivedMessage = JsonSerializer.Deserialize<BroadcastMessage>(receivedJson);

            Console.WriteLine($"Получено от {result.RemoteEndPoint}:");
            Console.WriteLine($"  Игрок: {receivedMessage.PlayerName}");
            Console.WriteLine($"  Время: {receivedMessage.Time}");
            Console.WriteLine($"  Позиция: ({receivedMessage.Position.X}, {receivedMessage.Position.Y})");
            Console.WriteLine();
        }
        catch (OperationCanceledException)
        {
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении: {ex.Message}");
        }
    }
});
await Task.WhenAll(sendTask, receiveTask);

public record BroadcastMessage(
    string PlayerName,
    DateTime Time,
    Position Position);

public record Position(int X, int Y);