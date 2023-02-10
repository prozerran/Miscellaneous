namespace WebApiNet7.Api.Extensions
{
    public static class ModuleExtensions
    {
        private static readonly List<IModule> _registeredModules = new();

        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            var modules = typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();

            _registeredModules.Clear();

            foreach (var module in modules)
            {
                module.RegisterModule(services);
                _registeredModules.Add(module);
            }

            return services;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach (var module in _registeredModules)
            {
                module.MapEndpoints(app);
            }
            return app;
        }
    }
}
