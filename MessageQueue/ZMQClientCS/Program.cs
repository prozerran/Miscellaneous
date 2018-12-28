using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;

using FlatBuffers;
using OrderBook.Book;

namespace ZMQClientCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string addr = "tcp://localhost:6666";
            Console.WriteLine("Client can connect to: {0}", addr);

            using (var rs = new RequestSocket())
            {
                rs.Connect(addr);

                while (true)
                {
                    string str = Console.ReadLine();
                    rs.SendFrame(str);

                    // read from network
                    var msg = rs.ReceiveFrameBytes();

                    // get our Flat Buffer
                    var order = Order.GetRootAsOrder(new ByteBuffer(msg));

                    // For C#, unlike other languages, most values (except for vectors and unions) are available as
                    // properties instead of accessor methods.

                    // Note: We did not set the `Mana` field explicitly, so we get back the default value.
                    Console.WriteLine(order.Id);
                    Console.WriteLine(order.Broker);
                    Console.WriteLine(order.Stockcode);
                    Console.WriteLine(order.Side);
                    Console.WriteLine($"Price {order.Price.Value.Spot}, {order.Price.Value.Open}, {order.Price.Value.Close}");

                    // Contracts
                    for (int i = 0; i < order.ContractsLength; i++)
                    {
                        Console.WriteLine($"Contract {order.Contracts(i).Value.Name}, {order.Contracts(i).Value.Price}");
                    }

                    // Get and test the `Equipped` FlatBuffer `union`.
                    if (order.StrategyType == Strategies.Contract)
                    {
                        var strategy = order.Strategy<Contract>().Value;
                        Console.WriteLine($"Strategy {strategy.Name}, {strategy.Price}");
                    }
                }
            }
        }
    }
}
