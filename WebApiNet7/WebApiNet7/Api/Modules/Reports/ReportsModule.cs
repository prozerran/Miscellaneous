using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Helpers;
using WebApiNet7.Services;

namespace WebApiNet7.Api.Modules.Reports
{
    public sealed class ReportsModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services) => services;

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            #region Queries

            // Get Memory Service Count
            endpoints.MapGet("/api/reports/service", async Task<Results<Ok<string>, NotFound>> (
                    IReportingService reportingService,
                    ILogger<ReportsModule> logger,
                    IMapper mapper) =>
                {
                    var id = await reportingService.GetCurrentCount();

                    logger.LogInformation($"Got Count {id}");

                    return TypedResults.Ok(id.ToString());
                })
                .RequireAuthorization()
                //.CacheOutput(o => o.NoCache())  // disable caching for NON-authenticated api
                .WithName("GetServiceCache")
                .WithTags("Reports");


            // Get Authenticated first
            endpoints.MapGet("/api/reports/{id:long}", async Task<Results<Ok<string>, NotFound>> (long id,
                    IMapper mapper) =>
                {
                    if (id <= 0)
                        return TypedResults.NotFound();

                    await Task.Delay(100);

                    return TypedResults.Ok("AUTHENTICATED!");
                })
                .RequireAuthorization()
                .WithName("GetRealtimeReports")
                .WithTags("Reports");

            // Cache Output
            endpoints.MapGet("/api/reports/{id:long}/hist", async Task<Results<Ok<string>, NotFound, ProblemHttpResult>> (long id,
                    DateTime from,
                    DateTime to,
                    IMapper mapper) =>
                {
                    if (id <= 0)
                        return TypedResults.NotFound();

                    if (DateTime.TryParse(from.ToString("dd-MM-yyyy"), out var fromDate) is false)
                        return ApiResults.Problem("wrong-date-format", "From must be in dd-MM-yyyy");

                    if (DateTime.TryParse(to.ToString("dd-MM-yyyy"), out var toDate) is false)
                        return ApiResults.Problem("wrong-date-format", "To must be in dd-MM-yyyy");

                    if (fromDate > toDate)
                        return ApiResults.Problem("wrong-date-range", "From date cannot be larger than To date");

                    await Task.Delay(100);

                    return TypedResults.Ok("CACHED!");
                })
                .RequireAuthorization()
                .CacheOutput("OutputCacheWithAuthPolicy")
                .WithName("GetHistoricalReports")
                .WithTags("Reports");


            #endregion

            return endpoints;
        }
    }
}
