using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using StackExchange.Redis;

/* Installing Redis Server
 * 
1) Open NuGet setup page and download Command-Line Utility (The latest version of the nuget.exe command-line tool is always available from https://nuget.org/nuget.exe)
2) Copy this file to somewhere (for example, C:\Downloads)
3) Start a command prompt as an Administrator and execute follow commands:
cd C:\Downloads
nuget.exe install redis-64
4) In the Downloads folder will be the latest version of Redis (C:\Downloads\Redis-64.2.8.19 in my case)
5) Run redis-server.exe and start working
*/

// https://stackexchange.github.io/StackExchange.Redis/
// https://stackexchange.github.io/StackExchange.Redis/Basics

namespace ModernCS
{
    public class Redis
    {
        private ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
        // ^^^ store and re-use this!!!

        public void Cache_Str()
        {
            IDatabase db = redis.GetDatabase();

            // SET
            db.StringSet("mykey", "HELLO_WORLD");

            // GET
            string value = db.StringGet("mykey");
            Console.WriteLine(value);
        }

        public void Cache_Binary()
        {
            IDatabase db = redis.GetDatabase();

            var v1 = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };

            db.StringSet("binKey", v1);
            byte[] v2 = db.StringGet("binKey");

            Debug.Assert(v2[0] == 0x20);
        }

        public void PublisherSubscriber()
        {
            Thread t1 = new Thread(new ThreadStart(Subscriber));
            t1.Start();

            Thread.Sleep(1000);     // put some lead time

            Thread t2 = new Thread(new ThreadStart(Publisher));
            t2.Start();

            Thread.Sleep(1000);     // put some lead time

            t1.Join();
            t2.Join();
        }

        private void Publisher()
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Publish("CHANNEL", "hello");
        }

        private void Subscriber()
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("CHANNEL", (channel, msg) => {
                Console.WriteLine("RECV : {0}", (string)msg);
            });
        }

        public void Access_Individual_Servers()
        {
            IServer server = redis.GetServer("localhost", 6379);

            // get endpoints
            EndPoint[] endpoints = redis.GetEndPoints();

            // example server commands
            DateTime lastSave = server.LastSave();
            ClientInfo[] clients = server.ClientList();
        }

        public void FireForget()
        {
            IDatabase db = redis.GetDatabase();

            // FireAndForget available to all methods
            // such as increment a string
            db.StringIncrement("ffkey", flags: CommandFlags.FireAndForget);
        }

        public async Task<string> Async()
        {
            IDatabase db = redis.GetDatabase();

            await db.StringSetAsync("askey", "HELLO_ASYNC");

            string value = await db.StringGetAsync("askey");
            //Console.WriteLine(value);

            return value;
        }

        public async Task<string> AsyncResult()
        {
            IDatabase db = redis.GetDatabase();

            await db.StringSetAsync("askey", "HELLO_ASYNC").ConfigureAwait(false);

            string value = await db.StringGetAsync("askey").ConfigureAwait(false);
            //Console.WriteLine(value);

            return value;
        }
    }
}
