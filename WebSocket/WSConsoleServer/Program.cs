using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSConsoleServer
{
    class Program
    {
        static async Task Run()
        {
            var server = new HttpListener();
            server.Prefixes.Add("http://localhost:8000/ws/");
            server.Start();
            Console.WriteLine("Websocket Server Started");

            var http = await server.GetContextAsync();
            Console.WriteLine("Connected");

            // check to see if connection is websocket
            if (!http.Request.IsWebSocketRequest)
            {
                http.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                http.Response.Close();
                Console.WriteLine("Not a websocket request");
                return;
            }

            // assume is websocket connection, start accepting
            var ws = await http.AcceptWebSocketAsync(null);

            // client connected, loop and serve by replying with current time
            for (var i = 0; i != 10; ++i)
            {
                await Task.Delay(1000);

                var time = DateTime.Now.ToLongTimeString();
                var buffer = Encoding.UTF8.GetBytes(time);
                var segment = new ArraySegment<byte>(buffer);

                Console.WriteLine(time);

                // send data back to client
                await ws.WebSocket.SendAsync(segment, WebSocketMessageType.Text,
                    true, CancellationToken.None);
            }

            // dont close abrutly, wait until client gracefully disconnects
            await ws.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                null, CancellationToken.None);

            Console.WriteLine("Closed");
        }

        static void Main(string[] args)
        {
            Run().Wait();
        }
    }
}
