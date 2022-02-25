using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace WebSocketClient
{
    public sealed class WSClient : WSClientBase
    {
        private static readonly Lazy<WSClient> lazy = new Lazy<WSClient>(() => new WSClient());
        public static WSClient Instance { get { return lazy.Value; } }

        private WSClient()
        { }

        public override void OnConnectionOpen(object sender, EventArgs e)
        {
            Send("Hi, there!");
        }

        public override void OnConnectionClosed(object sender, CloseEventArgs e)
        {
            var nm = new NotificationMessage
            {
                Summary = String.Format("WebSocket Close ({0})", e.Code),
                Body = e.Reason,
                Icon = "notification-message-im"
            };
            Notify(nm);
        }

        public override void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var nm = new NotificationMessage
            {
                Summary = "WebSocket Message",
                Body = !e.IsPing ? e.Data : "Received a ping.",
                Icon = "notification-message-im"
            };
            Notify(nm);
        }

        public override void OnError(object sender, ErrorEventArgs e)
        {
            var nm = new NotificationMessage
            {
                Summary = "WebSocket Error",
                Body = e.Message,
                Icon = "notification-message-im"
            };
            Notify(nm);
        }
    }
}
