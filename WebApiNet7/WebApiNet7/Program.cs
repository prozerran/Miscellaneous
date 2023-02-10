
using Serilog;
using Serilog.Events;

using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Policies;
using WebApiNet7.Services;
using WebApiNet7.Models;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region Build Host

    // Serilog
    builder.Host.UseSerilog((_, logger) =>
    {
        logger.MinimumLevel.Information()
            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.RazorPages", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.ViewFeatures", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.JwtBearer", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authorization.DefaultAuthorizationService", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler", LogEventLevel.Warning)
            .Enrich.FromLogContext();

        // Only output to console if we're in the Development Environment
        if (builder.Environment.IsDevelopment())
        {
            logger.WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] ({AssemblyName}) {SourceContext} {Message} {Exception} {NewLine}");
            logger.WriteTo.File("webapi_.log", rollingInterval: RollingInterval.Day);
        }
    });

    #endregion

    #region Build Configurations

    // configs
    builder.ConfigureOptions<JwtOptions>();

    #endregion

    #region Build Services

    // preliminaries
    builder.Services.Configure<JsonOptions>(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

    builder.Services.AddControllers().AddJsonOptions(options => 
    { 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });

    builder.Services.AddHttpContextAccessor();

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

    #endregion

    var app = builder.Build();

    #region App Use

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapEndpoints();

    app.UseCors(builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });

    app.UseSerilogRequestLogging();
    app.UseForwardedHeaders();
    app.UseHealthChecks("/health");
    app.UseStaticFiles();

    app.UseCookiePolicy();
    app.UseRouting();
    app.UseOutputCache();   // must be placed after CORS and Routing

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();

    #endregion

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
{ }