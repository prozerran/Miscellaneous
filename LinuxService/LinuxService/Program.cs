
using System;
using System.IO;
using System.Reflection;

using Serilog;
using Serilog.Events;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// For running on Linux
// https://devblogs.microsoft.com/dotnet/net-core-and-systemd/

// For running on Windows
// https://www.youtube.com/watch?v=PzrTiz_NRKA

// Project cannot yet compile into a single executable for either Windows or Linux
// Perhaps in future it can. To do so, in Publish -> Reset Target Runtime -> and set as produce single file.
// Then run ServiceInstaller.bat to install/uninstall service [for Windows]

namespace LinuxService
{
    public class Program
    {
        public static readonly string LogExe = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
        public static string LogPath = LogExe + @"\Logs\Service_.log";

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(LogPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a fatal issue starting the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseSystemd()     // for linux
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    var options = configuration.GetSection("ServerSetting").Get<ConfigSettings>();

                    

                    services.AddSingleton(options);
                    services.AddHttpClient<Worker>();
                    services.AddHostedService<Worker>();                    
                })
                .UseSerilog();
    }
}
