
using Serilog;

using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Policies;
using WebApiNet7.Services;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using WebApiNet7.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, lc) =>
        lc.WriteTo.Console()
        .MinimumLevel.Verbose());

    // configs
    builder.ConfigureOptions<JwtOptions>();

    // Add services to the container.
    builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
    builder.Services.AddUserJwtAuthentication(builder.Configuration.GetSectionAs<JwtOptions>());
    builder.Services.AddUserAuthorization();

    // hosted services
    builder.Services.AddSingleton<IReportingService, ReportingService>();   // 1 instance all users
    builder.Services.AddTransient<IRunnerService, RunnerService>();
    builder.Services.AddHostedService<ReportHostedService>();

    builder.Services.AddMemoryCache();

    // Auto Mapping
    builder.Services.AddAutoMapper(config =>
    {
        config.AddMaps(typeof(Program));
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddUserAuthentication();
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API Net 7", Version = "v1" });
    });

    builder.Services.AddHealthChecks();
    builder.Services.RegisterModules();

    builder.Services.AddOutputCache(options =>
    {
        options.AddBasePolicy(builder => builder.Cache());      // default cache expires in 1 minute
        //options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
        options.AddPolicy("OutputCacheWithAuthPolicy", OutputCacheWithAuthPolicy.Instance);
    });

    //// may need to disable this is you want output caching
    //builder.Services.AddMvc(options =>
    //{
    //    options.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
    //});


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapEndpoints();

    //app.UseCookiePolicy();
    app.UseRouting();
    app.UseOutputCache();   // must be placed after CORS and Routing

    app.UseAuthentication();
    app.UseAuthorization();
    //app.MapDefaultControllerRoute();

    app.Run();
}
catch (OperationCanceledException)
{
    Log.Information("Web Application terminated");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web Application failed to start");
}
finally
{
    Log.Information("Web Application shut down complete");
    Log.CloseAndFlush();
}

public partial class Program
{
}