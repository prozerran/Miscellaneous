namespace WebApiNet7.Services
{
    public interface IReportingService
    {
        Task<long> IncreaseCount(long id);

        Task<long> GetCurrentCount();

        Task<List<string>> GetReports(long id);

        Task<string> GetReport(long id);
    }
}
