using WebApiNet7.Api.Extensions;

namespace WebApiNet7.Api.Modules.Health
{
    public sealed class HealthModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services) => services;

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/version", () => TypedResults.Ok("V1.0"))
                .WithName("GetVersion")
                .WithTags("Health");

            endpoints.MapGet("/api/health", () => TypedResults.Ok("ok"))
                .ExcludeFromDescription();

            return endpoints;
        }
    }
}
