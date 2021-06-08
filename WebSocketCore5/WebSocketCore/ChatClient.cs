using Serilog;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketCore
{
    public class ChatClient
    {
        private WebSocket webSocket;

        public ChatClient(WebSocket webSocket)
        {
            this.webSocket = webSocket;
        }

        public async Task RunAsync()
        {
            var buffer = new byte[4086];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var str = Encoding.Default.GetString(buffer, 0, result.Count);
                Log.Information($"RECV: [{str}]");

                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);                
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
