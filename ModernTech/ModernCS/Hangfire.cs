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
    public class Hangfire
    {
        private BackgroundJobServerOptions options = null;

        public Hangfire()
        {
            //GlobalConfiguration.Configuration
            //    .UseColouredConsoleLogProvider()
            //    .UseSqlServerStorage("MyHangfireConnection");

            //JobStorage.Current = new SqlServerStorage("ConnectionStringName");

            // use internal/local memory as storage
            GlobalConfiguration.Configuration.UseMemoryStorage();

            // check interval schedular, default is 15 sec
            options = new BackgroundJobServerOptions
            {
                // set to 5 seconds instead
                SchedulePollingInterval = TimeSpan.FromSeconds(5)
            };
        }

        public void DoWork(string str)
        {
            Console.WriteLine($"{str} : {DateTime.Now.ToString()}");
        }

        public void HangFire()
        {
            using (var server = new BackgroundJobServer(options))
            {
                // run once
                var jobId = BackgroundJob.Enqueue(() => this.DoWork("BEG"));

                // delayed
                jobId = BackgroundJob.Schedule(() => this.DoWork("DEL"), TimeSpan.FromSeconds(1));

                // recurring
                RecurringJob.AddOrUpdate(() => this.DoWork("REC"), Cron.Minutely());

                // final, until all jobs complete
                BackgroundJob.ContinueWith(jobId, () => this.DoWork("END"));

                Console.WriteLine("Hangfire Server started. Press any key to exit...");

                // pause until input
                Console.Read();
            }
        }
    }
}
