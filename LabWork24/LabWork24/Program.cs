using System.Security.AccessControl;

class Program
{
    static string commnonVar = "";

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите задание для запуска:");
            Console.WriteLine("1 - 5.1 Потоки: 0 и 1");
            Console.WriteLine("2 - 5.2 Приоритет потоков");
            Console.WriteLine("3 - 5.3 Обмен данными между потоками");
            Console.WriteLine("4 - 5.4 Пул потоков (делители)");
            Console.WriteLine("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunTask5_1();
                    break;
                case "2":
                    RunTask5_2();
                    break;
                case "3":
                    RunTask5_3();
                    break;
                case "4":
                    RunTask5_4();
                    break;
            }
        }
    }
    //Задание 1
    static void RunTask5_1()
    {
        Console.WriteLine("Запуск задания 5.1 ....");
        Thread t = new Thread(() =>
        {
            while(true)
            {
                Console.WriteLine("1");
                Thread.Sleep(500);
            }
        });
        t.Start();

        while (true)
        {
            Console.WriteLine("0");
            Thread.Sleep(1000);
        }
    }

    //Задание 2

    static void RunTask5_2()
    {
        Console.WriteLine("Запуск задания 5.2 ....");
        Thread t1 = new (WriteString);
        Thread t2 = new (WriteString);
        Thread t3 = new (WriteString);
        Thread t4 = new (WriteString);

        t1.Priority = ThreadPriority.Lowest;
        t2.Priority = ThreadPriority.BelowNormal;
        t3.Priority = ThreadPriority.AboveNormal;
        t4.Priority = ThreadPriority.Highest;

        t1.Start(1);
        t2.Start(2);
        t3.Start(3);
        t4.Start(4);
    }
    static void WriteString(object n)
    {
        int num = (int)n;
        Console.WriteLine($"\nПоток {num} запущен");
        for(int i = 0; i < 1000; i++)
        {
            Console.WriteLine(num);
        }
        Console.WriteLine($"\nПоток {num} завершён");
    }

    static void RunTask5_3()
    {
        Console.WriteLine("Запуск задание 5.3 ...");
        Thread t = new Thread(() =>
        { 
            while(commnonVar != "x")
            {
                Console.WriteLine("Ожидание переменной = 'x' ...");
                Thread.Sleep(1000);
            }
            Console.WriteLine("MyThread завершён - получено 'x'");
        });
        t.Start();

        while (commnonVar != "x")
        {
            Console.WriteLine("Введите значение переменной: ");
            commnonVar = Console.ReadLine();
        }
    }

    static void RunTask5_4()
    {
        Console.WriteLine("Запуск задания 5.4 ...");
        for(int i = 0; i < 10; i++)
        {
            int number = 10 + i;
            ThreadPool.QueueUserWorkItem(FindDivisors, number);
        }
        Console.WriteLine("Нажмите Enter для возврата в меню...");
        Console.ReadLine();
    }

    static void FindDivisors(object numObj)
    {
        int number = (int)numObj;
        for (int i = 1; i < number; i++)
        {
            if(number % i == 0)
            {
                Console.WriteLine($"{number} делится нацело на {i}");
            }
        }
    }
}