using System;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebSocketServer
{
    public class WSService : WebSocketBehavior
    {
        private string _id;
        private string _prefix;

        public WSService() : this(null)
        { }

        public WSService(string prefix)
        {
            _prefix = !prefix.IsNullOrEmpty() ? prefix : "anon#";
        }

        private string GetId()
        {
            var name = Context.QueryString["id"];
            return !name.IsNullOrEmpty() ? name : _prefix + ID;     // ID contains the unique session id
        }

        //private static int GetNumber()
        //{
        //    return Interlocked.Increment(ref _number);
        //}

        protected override void OnClose(CloseEventArgs e)
        {
            Sessions.Broadcast(string.Format("{0} got logged off...", _id));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine(e.Data);
            Send(string.Format("{0}: {1}", _id, e.Data));
        }

        protected override void OnOpen()
        {
            _id = GetId();
        }

        public void BroadCast(string msg)
        {
            Sessions.Broadcast(string.Format("{0}: {1}", _id, msg));
        }
    }
}
