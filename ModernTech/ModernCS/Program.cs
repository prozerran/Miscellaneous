using Hangfire;
using Hangfire.SqlServer;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernCS
{
    class Program
    {
        static void Redis()
        {
            Redis red = new Redis();
            red.Cache_Str();
            red.Cache_Binary();
            red.PublisherSubscriber();
        }

        static void Hangfire()
        {
            Hangfire hf = new Hangfire();
            hf.HangFire();
        }

        static void Main(string[] args)
        {
            Program.Hangfire();
        }
    }
}
