using System.Net.Sockets;

Console.Write("Введите путь к файлу изображения: ");
string filePath = Console.ReadLine()!;

if (!File.Exists(filePath))
{
    Console.WriteLine("Файл не найден!");
    return;
}

try
{
    using var client = new TcpClient("localhost", 8888);
    using var stream = client.GetStream();

    byte[] fileBytes = File.ReadAllBytes(filePath);
    long fileSize = fileBytes.LongLength;

    await stream.WriteAsync(BitConverter.GetBytes(fileSize), 0, 8);
    await stream.WriteAsync(fileBytes, 0, fileBytes.Length);

    byte[] sizeBytes = new byte[8];
    await stream.ReadExactlyAsync(sizeBytes, 0, 8);
    long responseSize = BitConverter.ToInt64(sizeBytes, 0);

    byte[] responseBytes = new byte[responseSize];
    await stream.ReadExactlyAsync(responseBytes, 0, responseBytes.Length);

    string resultPath = Path.Combine(
        Path.GetDirectoryName(filePath)!,
        Path.GetFileNameWithoutExtension(filePath) + "_resized.jpg");

    await File.WriteAllBytesAsync(resultPath, responseBytes);
    Console.WriteLine($"Изображение сохранено как: {resultPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}