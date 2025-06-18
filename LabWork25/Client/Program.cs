using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LabWork25_1
{
    class Client
    {
        static void Main()
        {
            TcpClient tcpClient = new TcpClient("127.0.0.1", 5000);
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            while (true)
            {
                string message = reader.ReadLine();
                Console.WriteLine("Сервер: " + message);
            }
        }
    }
}
