using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string fileName = "";
        string searchText = "";

        // Парсим аргументы
        foreach (var arg in args)
        {
            if (arg.StartsWith("-file:"))
            {
                fileName = arg.Substring(6);
            }
            else if (arg.StartsWith("-text:"))
            {
                searchText = arg.Substring(6);
            }
        }

        // Проверяем наличие обязательных параметров
        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(searchText))
        {
            Console.WriteLine("Both -file and -text parameters are required.");
            return;
        }

        // Проверяем существование файла
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"The file \"{fileName}\" does not exist.");
            return;
        }

        try
        {
            // Читаем файл построчно
            var lines = File.ReadAllLines(fileName);
            bool found = false;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    Console.WriteLine($"Line {i + 1}: {lines[i]}");
                }
            }

            if (!found)
            {
                Console.WriteLine($"Text \"{searchText}\" not found in file \"{fileName}\".");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}