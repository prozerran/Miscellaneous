using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

namespace ZeroClientCS
{
    class Program
    {
        static void Main(string[] args)
        {           
            using (var rs = new RequestSocket(">tcp://localhost:5555"))
            {
                int i = 0;

                while (true)
                {
                    Console.WriteLine(">> Sending {0} 'Hello'", i++);
                    rs.SendFrame("Hello");

                    var message = rs.ReceiveFrameString();
                    Console.WriteLine("\t<< Received '{0}'", message);
                }
            }
        }
    }
}
