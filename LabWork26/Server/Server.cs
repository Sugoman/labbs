using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;

var listener = new TcpListener(IPAddress.Any, 8888);
listener.Start();
Console.WriteLine("Сервер запущен. Ожидание подключений...");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    Console.WriteLine("Клиент подключен.");
    _ = HandleClientAsync(client);
}

async Task HandleClientAsync(TcpClient client)
{
    try
    {
        using (client)
        using (var stream = client.GetStream())
        {
            byte[] sizeBytes = new byte[8];
            await stream.ReadExactlyAsync(sizeBytes, 0, 8);
            long fileSize = BitConverter.ToInt64(sizeBytes, 0);

            byte[] imageBuffer = new byte[fileSize];
            await stream.ReadExactlyAsync(imageBuffer, 0, imageBuffer.Length);

            byte[] resizedBytes;
            using (var inputStream = new MemoryStream(imageBuffer))
            using (var originalImage = new Bitmap(inputStream))
            {
                int newWidth = originalImage.Width / 2;
                int newHeight = originalImage.Height / 2;
                using var resizedImage = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }
                using var outputStream = new MemoryStream();
                resizedImage.Save(outputStream, ImageFormat.Jpeg);
                resizedBytes = outputStream.ToArray();
            }

            await stream.WriteAsync(BitConverter.GetBytes(resizedBytes.LongLength), 0, 8);
            await stream.WriteAsync(resizedBytes, 0, resizedBytes.Length);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Клиент отключен.");
    }
}