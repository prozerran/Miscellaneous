
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Serilog;

namespace WebSocketCore
{
    public class SocketWare
    {
        private RequestDelegate next;

        public SocketWare(RequestDelegate _next)
        {
            this.next = _next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                await next(context);
            else
            {
                if (context.Request.Path == "/ws")
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        if (webSocket.State == WebSocketState.Open)
                        {
                            await RunEchoServerAsync(webSocket);
                        }
                    }
                }
            }
        }

        private async Task RunEchoServerAsync(WebSocket webSocket)
        {
            try
            {
                var client = new ChatClient(webSocket);
                await client.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
