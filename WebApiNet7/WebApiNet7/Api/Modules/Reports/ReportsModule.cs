using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApis.Extensions.Binding;
using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Helpers;
using WebApiNet7.Api.Modules.Reports.Models;
using WebApiNet7.Services;

namespace WebApiNet7.Api.Modules.Reports
{
    public sealed class ReportsModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services) => services;

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            #region Queries

            // Get General Cached
            endpoints.MapGet("/api/reports/general", Results<Ok<long>, NotFound> (
                    IHttpContextAccessor httpContextAccessor,
                    IMapper mapper) =>
                {
                    var id = httpContextAccessor.GetUserId();

                    return TypedResults.Ok(id);
                })
                .WithName("GetGeneral")
                .WithTags("Reports");


            // Get Authenticated NOT Cached
            endpoints.MapGet("/api/reports/service", async Task<Results<Ok<string>, NotFound>> (
                    IReportingService reportingService,
                    ILogger<ReportsModule> logger,
                    IMapper mapper) =>
                {
                    var id = await reportingService.GetCurrentCount();

                    logger.LogInformation($"Got Count {id}");

                    return TypedResults.Ok($"Authenticated NOT Cached! [{id}]");
                })
                .RequireAuthorization()
                //.CacheOutput(o => o.NoCache())  // disable caching for NON-authenticated api
                .WithName("GetServiceCache")
                .WithTags("Reports");


            // Get Authenticated Cached
            endpoints.MapGet("/api/reports/{id:long}", async Task<Results<Ok<string>, NotFound>> (long id,
                    IReportingService reportingService,
                    IMapper mapper) =>
                {
                    var count = await reportingService.GetCurrentCount();

                    return TypedResults.Ok($"Authenticated Cached! [{count}]");
                })
                .RequireAuthorization()
                .CacheOutput("OutputCacheWithAuthPolicy")
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

                    return id > 0
                        ? TypedResults.Ok($"Ok, Got {id}")
                        : TypedResults.Problem("unknown-problem", "The service may be corrupt");
                })
                .RequireAuthorization()
                .CacheOutput("OutputCacheWithAuthPolicy")
                .WithName("GetHistoricalReports")
                .WithTags("Reports");


            #endregion

            #region Commands

            // Post Input
            endpoints.MapPost("/api/reports/input", async Task<Results<ValidationProblem, Ok<List<Records>>, ProblemHttpResult>> (Validated<InputModel> request,
                    IHttpContextAccessor httpContextAccessor,
                    IReportingService reportingService,
                    IMapper mapper) =>
                {
                    if (!request.IsValid || request.Value == null)
                        return TypedResults.ValidationProblem(request.Errors);

                    try
                    {
                        var id = await reportingService.GetCurrentCount();

                        var records = new List<RecordsModel>
                        {
                            new RecordsModel { Id = 1, Name = "Jenny" },
                            new RecordsModel { Id = 2, Name = "Penny" },
                            new RecordsModel { Id = 3, Name = "Kenny" },
                            new RecordsModel { Id = 4, Name = "Denny" },
                            new RecordsModel { Id = 5, Name = "Benny" },
                            new RecordsModel { Id = Convert.ToInt32(id), Name = "CACHED?" },
                        };

                        return TypedResults.Ok(records
                            .Select(mapper.Map<Records>)
                            .ToList());
                    }
                    catch (Exception ex)
                    {
                        return ApiResults.Problem(ex.HResult.ToString(), ex.Message);
                    }
                })
                .WithName("GetInputModel")
                .WithTags("Reports")
                .Accepts<InputModel>("application/json");

            #endregion

            return endpoints;
        }
    }
}
