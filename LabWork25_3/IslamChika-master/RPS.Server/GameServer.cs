using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPS.Server
{
    public class GameServer
    {
        TcpListener server;

        TcpClient player1;
        TcpClient player2;

        public GameServer()
        {
            server = new TcpListener(IPAddress.Any, 5000);
        }

        public async Task Start()
        {
            server.Start();
            Console.WriteLine("Ожидание игроков...");

            player1 = await server.AcceptTcpClientAsync();
            Console.WriteLine("Игрок 1 подключён");

            player2 = await server.AcceptTcpClientAsync();
            Console.WriteLine("Игрок 2 подключён");
            while (true) 
            {
                await HandleGame();
            }
           

        }

        private async Task HandleGame()
        {
            NetworkStream stream1 = player1.GetStream();
            NetworkStream stream2 = player2.GetStream();
            string move1 = await ReadMoveAsync(stream1);
            string move2 = await ReadMoveAsync(stream2);

            int result = GetWinner(move1, move2);

            await SendMessageAsync(stream1, GetWinnerMessage(1, result));
            await SendMessageAsync(stream2, GetWinnerMessage(2, result));
        }

        private static async Task SendMessageAsync(NetworkStream stream, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length); 
        }
            private static async Task<String> ReadMoveAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        private int GetWinner(string move1, string move2)
        {
            if (move1 == move2) return 0;
            if (move1 == "камень" && move2 == "ножницы"||
                move1 == "ножницы" && move2 == "бумага" ||
                move1 == "бумага" && move2 == "камень")
                return 1;
            if (move1 == "муслим" && move2 == "камень" ||
                move1 == "муслим" && move2 == "ножницы" ||
                move1 == "муслим" && move2 == "бумага")
                return 3;
            else
                return 2;
        }

        private string GetWinnerMessage(int player, int value)
        {
            if (value == 0) return "Ничья";
            if (value == player) return "Вы победели";
            if (value == 3) return "АЛЛАХУ АКБАР";
            else return "Вы проиграли";
        }
    }
}
