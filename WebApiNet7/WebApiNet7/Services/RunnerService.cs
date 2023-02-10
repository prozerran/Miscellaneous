using AutoMapper;

namespace WebApiNet7.Services
{
    public class RunnerService : IRunnerService
    {
        private readonly ILogger<RunnerService> _logger;
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public RunnerService(
            ILogger<RunnerService> logger,
            IReportingService reportingService,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async Task Execute(long id)
        {
            try
            {
                await _reportingService.IncreaseCount(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while executing job {id}");
            }
        }
    }
}
