using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ;
using RabbitMQ.Client;

namespace RabbitClientCS
{
    class CRabbitMQ
    {
        private string queue = "TESTQ";
        private ConnectionFactory connectionFactory = null;
        private IConnection connection = null;

        ~CRabbitMQ()
        {
            Disconnect();
        }

        public IConnection Connect(string host)
        {
            connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = host;
            //connectionFactory.Port = 5555;
            //connectionFactory.UserName = userName;
            //connectionFactory.Password = password;
            connection = connectionFactory.CreateConnection();
            return connection;
        }

        public void Disconnect()
        {
            if (connection.IsOpen)
            {
                connection.Close();
                connection = null;
            }
        }

        public int Send(string str)
        {
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue, false, false, false, null);
                channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(str));
            }
            return str.Length;
        }

        public int Recv(ref string str)
        {
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue, false, false, false, null);
                var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue, true);
                if (result != null)
                {
                    str = Encoding.UTF8.GetString(result.Body);
                }
            }
            return str.Length;
        }
    }
}
