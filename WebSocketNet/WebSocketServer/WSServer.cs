using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace WebSocketServer
{
    public sealed class WSServer : WSServerBase
    {
        private static readonly Lazy<WSServer> lazy = new Lazy<WSServer>(() => new WSServer());
        public static WSServer Instance { get { return lazy.Value; } }

        private WSServer()
        { }

        public override void OnHttpGet(object sender, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;

            var path = req.RawUrl;
            if (path == "/")
                path += "index.html";

            byte[] contents;
            if (!e.TryReadFile(path, out contents))
            {
                res.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            if (path.EndsWith(".html"))
            {
                res.ContentType = "text/html";
                res.ContentEncoding = Encoding.UTF8;
            }
            else if (path.EndsWith(".js"))
            {
                res.ContentType = "application/javascript";
                res.ContentEncoding = Encoding.UTF8;
            }
            res.WriteContent(contents);
        }
    }
}
