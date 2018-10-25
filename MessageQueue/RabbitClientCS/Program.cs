using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RabbitMQ;
using RabbitMQ.Client;

// Need to install RabbitMQ Server
// http://www.rabbitmq.com/

namespace RabbitClientCS
{
    class Program
    {
        static void Send()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = "Hello World from TIM!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        static void Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);
            }
            Thread.Sleep(1000);            
        }

        static void Main(string[] args)
        {
            CRabbitMQ rmq = new CRabbitMQ();
            rmq.Connect("localhost");

            if (args[0] == "-s")
            {
                rmq.Send(args[1]);
            }
            else
            {
                string reply = string.Empty;
                rmq.Recv(ref reply);
                Console.WriteLine(reply);
            }
            return;
        }
    }
}
