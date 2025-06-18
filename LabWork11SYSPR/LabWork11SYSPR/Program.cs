Console.WriteLine("Введите имя файла(Только работает путь): ");
string fileName = Console.ReadLine();

if (File.Exists(fileName))
{
    string content = File.ReadAllText(fileName);
    Console.WriteLine("Содержимое файла");
    Console.WriteLine(content);
}
else
{
    Console.WriteLine("Файл не найден");
}

using (StreamWriter sw = new StreamWriter(fileName, true))
{
    string input;
    Console.WriteLine("Введите строки для добавления (для выхода 'end'): ");
    while((input = Console.ReadLine()) != "end")
    {
        sw.WriteLine(input);
    }
}
