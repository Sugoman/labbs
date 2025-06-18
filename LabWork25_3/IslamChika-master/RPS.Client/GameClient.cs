using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RPS.Client
{
    public class GameClient
    {
        TcpClient client;
        NetworkStream stream;

        public bool IsConnected => client?.Connected ?? false;

        public async Task Start(string serverAddress, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(serverAddress, port);
            stream = client.GetStream();
        }

        public async Task SendMoveAsync(string move)
        {
            byte[] data = Encoding.UTF8.GetBytes(move);
            await stream.WriteAsync(data, 0, data.Length);
        }

        public async Task<String> ReadResultAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }
}
