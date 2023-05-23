
using System.Net.WebSockets;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RealWebSocket
{
    //public class LambdaContext : ILambdaContext
    //{
    //    public string AwsRequestId { get; set; }
    //    public IClientContext ClientContext { get; set; }
    //    public string FunctionName { get; set; }
    //    public string FunctionVersion { get; set; }
    //    public ICognitoIdentity Identity { get; set; }
    //    public string InvokedFunctionArn { get; set; }
    //    public ILambdaLogger Logger { get; set; }
    //    public string LogGroupName { get; set; }
    //    public string LogStreamName { get; set; }
    //    public int MemoryLimitInMB { get; set; }
    //    public TimeSpan RemainingTime { get; set; }

    //    public CancellationToken CancellationToken { get; set; }
    //}


    public class WebSocketHandler
    {
        public async Task Handle(WebSocket webSocket, APIGatewayProxyRequest request, ILambdaContext context)
        {
            //var cancellationToken = context.CancellationToken;
            var cancellationToken = new CancellationToken();
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

            while (!result.CloseStatus.HasValue && !cancellationToken.IsCancellationRequested)
            {
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var payload = $"{request.RequestContext.ConnectionId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
                        await SendMessage(webSocket, payload, cancellationToken);
                        break;
                    case WebSocketMessageType.Close:
                        await onDisconnect(request.RequestContext.ConnectionId);
                        break;
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            }
        }

        public async Task onConnect(string connectionId)
        {
            // Your onConnect handler here
        }

        public async Task onDisconnect(string connectionId)
        {
            // Your onDisconnect handler here
        }

        public async Task SendMessage(WebSocket webSocket, string message, CancellationToken cancellationToken)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}
