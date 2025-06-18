using System.Diagnostics;
using System.Drawing;

class Program
{
    static object consolelock = new object();


    static void Main(string[] args)
    {
        string inputFolder = @"C:\Temp\.ispp21\Картинки 2";

        string outputFolder = Path.Combine(inputFolder, "output");

        Directory.CreateDirectory(outputFolder);

        string[] files = Directory.GetFiles(inputFolder, "*.*")
                                  .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                                  || f.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                                  || f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                                  .ToArray();

        int total = files.Length;
        int processed = 0;

        Console.WriteLine($"Найдено {total} изображений. Начинаем обработку...");

        Parallel.ForEach(files, file =>
        {
            try
            {
                using (Bitmap bmp = new Bitmap(file))
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            Color pixel = bmp.GetPixel(x, y);
                            Color Inverted = Color.FromArgb(
                                255 - pixel.R,
                                255 - pixel.G,
                                255 - pixel.B);
                            bmp.SetPixel(x, y, Inverted);
                        }
                    }
                    string fileName = Path.GetFileName(file);
                    string outputPath = Path.Combine(outputFolder, fileName);
                    bmp.Save(outputPath);
                }
                int done = Interlocked.Increment(ref processed);

                lock (consolelock)
                {
                    Console.CursorLeft = 0;
                    Console.WriteLine($"Обработано: {done * 100 / total}% ({done}/{total})");
                }
            }
            catch (Exception ex)
            {
                lock (consolelock)
                {
                    Console.WriteLine($"\nОшибка при обработке файла {file}: {ex.Message}");
                }
            }
        });
        Console.WriteLine("\nГотово! Все изображения обработаны.");
    }
}