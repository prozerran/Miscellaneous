namespace WebApiNet7.Services
{
    public class ReportingService : IReportingService
    {
        private long current_count = 0;

        public async Task<long> GetCurrentCount()
        {
            return await Task.FromResult(current_count);
        }

        public async Task<long> IncreaseCount(long id)
        {
            current_count = id;
            return await Task.FromResult(current_count);
        }

        public async Task<string> GetReport(long id)
        {
            await Task.Delay(10);
            return id.ToString();
        }

        public async Task<List<string>> GetReports(long id)
        {
            await Task.Delay(10);
            return new List<string> { "1", "2", "3" };
        }
    }
}
