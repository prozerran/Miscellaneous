
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Serilog;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LinuxService
{
    public class Worker : BackgroundService
    {
        public static string LogPath = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Service_.log";

        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConfigSettings _options;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger, IHttpClientFactory clientFactory, ConfigSettings options)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _options = options;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = _clientFactory.CreateClient();
            Log.Information($"Service: [{_options.Name}] has started.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            Log.Information($"Service stopped.");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync("http://yeehsu.freeshell.org/secret.html");

                if (result.IsSuccessStatusCode)
                {
                    Log.Information($"Website is running. Status Code = {result.StatusCode}");
                }
                else
                {
                    Log.Error($"The website is down. Status Code = {result.StatusCode}");
                }
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
