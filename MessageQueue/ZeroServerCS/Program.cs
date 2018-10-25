using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

namespace ZeroServerCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server started at: >tcp://localhost:5555");

            using (var rs = new ResponseSocket("@tcp://*:5555"))
            {
                int i = 0;

                while (true)
                {
                    var message = rs.ReceiveFrameString();
                    Console.WriteLine("Server Received {0} '{1}'", i++, message);

                    rs.SendFrame("World");
                    Thread.Sleep(1000);
                }
            }

            /*
            // Create
            using (var context = new ZContext())
            using (var responder = new ZSocket(context, ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5555");

                while (true)
                {
                    // Receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        Console.WriteLine("Received {0}", request.ReadString());

                        // Do some work
                        Thread.Sleep(1);

                        // Send
                        responder.Send(new ZFrame(name));
                    }
                }
            }
            */
        }
    }
}
