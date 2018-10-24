using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSConsoleClient
{
    class Program
    {
        static async Task Run()
        {
            ClientWebSocket ws = null;
            ws = new ClientWebSocket();
            string url = "ws://localhost:8000/ws/";

            await ws.ConnectAsync(new Uri(url), CancellationToken.None);
            Console.WriteLine("Connected");

            for (var i = 0; i != 10; ++i)
            {
                await Task.Delay(1000);

                byte[] buffer = new byte[1024];
                var segment = new ArraySegment<byte>(buffer);

                var result = await ws.ReceiveAsync(segment, CancellationToken.None);

                string str = Encoding.ASCII.GetString(segment.Array,
                             segment.Offset, result.Count);

                Console.WriteLine(str);
            }

            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure,
                null, CancellationToken.None);

            Console.WriteLine("Closed");
        }

        static void Main(string[] args)
        {
            Run().Wait();
        }
    }
}
