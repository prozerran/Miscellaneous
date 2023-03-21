



using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class GreetingService : IGreetingService
{
    private readonly ILogger<GreetingService> _logger;
    private readonly IConfiguration _config;

    public GreetingService(ILogger<GreetingService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public void Run()
    {
        for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
        {
            // for logging purpose, use the following to grep
            _logger.LogInformation("We are running on: {runNumber}", i);
        }
    }
}