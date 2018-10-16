
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    class Queue
    {
        static void Main(string[] args)
        {
            var storage = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            var queueClient = storage.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("tasks");

            queue.CreateIfNotExists();

            var mesg = new CloudQueueMessage("Hello World");
            queue.AddMessage(mesg);

            mesg = queue.PeekMessage();
            Console.WriteLine(mesg.AsString);

            mesg = queue.GetMessage();
            queue.DeleteMessage(mesg);

            Console.ReadKey();
        }
    }
}
