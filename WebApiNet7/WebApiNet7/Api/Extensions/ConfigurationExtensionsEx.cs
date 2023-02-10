namespace WebApiNet7.Api.Extensions
{
    public static class ConfigurationExtensionsEx
    {
        public static T GetSectionAs<T>(this IConfiguration configuration) where T : notnull, new()
        {
            var name = typeof(T).Name;
            return configuration.GetSection(name).Get<T>() ?? new T();
        }

        public static WebApplicationBuilder ConfigureOptions<T>(this WebApplicationBuilder webApplicationBuilder) where T : class
        {
            webApplicationBuilder.Services.ConfigureOptionsExt<T>(webApplicationBuilder.Configuration);
            return webApplicationBuilder;
        }

        public static void ConfigureOptionsExt<T>(this IServiceCollection services, IConfiguration cfg) where T : class
        {
            var name = typeof(T).Name;
            services.Configure<T>(cfg.GetSection(name));
        }
    }
}
