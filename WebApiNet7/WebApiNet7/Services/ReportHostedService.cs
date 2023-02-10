using System.Diagnostics;

namespace WebApiNet7.Services
{
    public class ReportHostedService : BackgroundService
    {
        private readonly ILogger<ReportHostedService> _logger;

        private readonly IServiceScopeFactory _factory;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(1);

        public ReportHostedService(ILogger<ReportHostedService> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int count = 0;
            using var timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using var asyncScope = _factory.CreateAsyncScope();

                    var service = asyncScope.ServiceProvider.GetRequiredService<IRunnerService>();

                    await service.Execute(count++);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to execute HostedService");
                }
            }
        }
    }
}
