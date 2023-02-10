namespace WebApiNet7.Api.Extensions
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection services);
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
