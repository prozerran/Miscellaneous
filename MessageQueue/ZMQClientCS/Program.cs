﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetMQ;
using NetMQ.Sockets;
using NetMQ.Security;

using FlatBuffers;
using OrderBook.Book;

// 1. Uses ZeroMQ
// 2. Uses FlatBuffers
//		- https://google.github.io/flatbuffers/flatbuffers_guide_use_java_c-sharp.html
//		- /drives/c/Users/tim.hsu/Documents/Github/vcpkg/packages/flatbuffers_x86-windows/tools/flatbuffers/flatc.exe -n FBSchema.fbs
//  3. Security
//      - https://somdoron.com/2013/05/securing-netmq/
//      - https://github.com/NetMQ/NetMQ.Security/blob/master/tests/NetMQ.Security.Tests/SecureChannelTests.cs

namespace ZMQClientCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string addr = "tcp://localhost:6666";
            Console.WriteLine("Client can connect to: {0}", addr);

            using (var rs = new RequestSocket())    // 		// REQ must be send/recv...send/recv in this order
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

                    // Instruments
                    for (int i = 0; i < order.InstrumentsLength; i++)
                    {
                        Console.WriteLine($"Instrument {order.Instruments(i).Value.Tag}, {order.Instruments(i).Value.Value}, {order.Instruments(i).Value.BlobLength}");
                        byte[] bb = order.Instruments(i).Value.GetBlobBytes().Value.Array;
                        //foreach (byte b in bb)
                        //{
                        //    Console.Write($" {b.ToString()}");
                        //}
                        //Console.WriteLine();
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
