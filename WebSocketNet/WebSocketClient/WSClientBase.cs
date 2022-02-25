using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace WebSocketClient
{
    public abstract class WSClientBase
    {
        protected readonly string WEB_SOCK_HOST = "localhost";
        protected readonly int WEB_SOCK_PORT = 4649;

        private WebSocket ws = null;
        private Notifier nf = null;

        public bool IsAlive()
        {
            return ws.IsAlive;
        }

        public void Reconnect()
        {
            ws.Connect();
        }

        public virtual void Start(string id)
        {
            string wsconn = string.Format($"ws://{WEB_SOCK_HOST}:{WEB_SOCK_PORT}/WSService?id={id}");

            // Create a new instance of the WebSocket class.
            //
            // The WebSocket class inherits the System.IDisposable interface, so you can
            // use the using statement. And the WebSocket connection will be closed with
            // close status 1001 (going away) when the control leaves the using block.
            //
            // If you would like to connect to the server with the secure connection,
            // you should create a new instance with a wss scheme WebSocket URL.

            nf = new Notifier();
            //using (var ws = new WebSocket ("ws://echo.websocket.org"))
            //using (var ws = new WebSocket ("wss://echo.websocket.org"))
            //using (var ws = new WebSocket ("ws://localhost:4649/Echo"))
            //using (var ws = new WebSocket ("wss://localhost:5963/Echo"))
            //using (var ws = new WebSocket ("ws://localhost:4649/Echo?name=nobita"))
            //using (var ws = new WebSocket ("wss://localhost:5963/Echo?name=nobita"))
            //using (var ws = new WebSocket ("ws://localhost:4649/Chat"))
            //using (var ws = new WebSocket ("wss://localhost:5963/Chat"))
            //using (ws = new WebSocket(wsconn))
            //using (var ws = new WebSocket ("wss://localhost:5963/Chat?name=nobita"))
            ws = new WebSocket(wsconn);
            {
                // Set the WebSocket events.
                ws.OnOpen += new EventHandler(OnConnectionOpen);
                ws.OnClose += new EventHandler<CloseEventArgs>(OnConnectionClosed);
                ws.OnMessage += new EventHandler<MessageEventArgs>(OnMessageReceived);
                ws.OnError += new EventHandler<ErrorEventArgs>(OnError);

#if DEBUG
                // To change the logging level.
                //ws.Log.Level = LogLevel.Trace;

                // To change the wait time for the response to the Ping or Close.
                //ws.WaitTime = TimeSpan.FromSeconds (10);

                // To emit a WebSocket.OnMessage event when receives a ping.
                //ws.EmitOnPing = true;
#endif
                // To enable the Per-message Compression extension.
                //ws.Compression = CompressionMethod.Deflate;

                // To validate the server certificate.
                /*
                ws.SslConfiguration.ServerCertificateValidationCallback =
                  (sender, certificate, chain, sslPolicyErrors) => {
                    ws.Log.Debug (
                      String.Format (
                        "Certificate:\n- Issuer: {0}\n- Subject: {1}",
                        certificate.Issuer,
                        certificate.Subject
                      )
                    );

                    return true; // If the server certificate is valid.
                  };
                 */

                // To send the credentials for the HTTP Authentication (Basic/Digest).
                //ws.SetCredentials ("nobita", "password", false);

                // To send the Origin header.
                //ws.Origin = "http://localhost:4649";

                // To send the cookies.
                //ws.SetCookie (new Cookie ("name", "nobita"));
                //ws.SetCookie (new Cookie ("roles", "\"idiot, gunfighter\""));

                // To connect through the HTTP Proxy server.
                //ws.SetProxy ("http://localhost:3128", "nobita", "password");

                // To enable the redirection.
                //ws.EnableRedirection = true;

                // Connect to the server.
                Reconnect();

                // Connect to the server asynchronously.
                //ws.ConnectAsync ();
            }
        }

        public virtual void Stop()
        {
            nf.Close();
            ws.Close();
            ws = null;
        }

        public virtual void Send(string msg)
        {
            if (!IsAlive())
                Reconnect();

            // Send a text message.
            ws.Send(msg);
        }

        public virtual void Notify(NotificationMessage nm)
        {
            nf.Notify(nm);
        }

        public abstract void OnConnectionOpen(object sender, EventArgs e);
        public abstract void OnConnectionClosed(object sender, CloseEventArgs e);
        public abstract void OnMessageReceived(object sender, MessageEventArgs e);
        public abstract void OnError(object sender, ErrorEventArgs e);
    }
}
