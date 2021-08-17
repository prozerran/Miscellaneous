using CustomAuthorizationFailureResponse.Authentication;
using CustomAuthorizationFailureResponse.Authorization;
using CustomAuthorizationFailureResponse.Authorization.Handlers;
using CustomAuthorizationFailureResponse.Authorization.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiNetCore5.Filters;
using WebApiNetCore5.Models;

namespace WebApiNetCore5
{
    public class Startup
    {
        public const string CustomForbiddenMessage = "YOU ARE NOT AUTHORIZED!";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddAuthentication(SampleAuthenticationSchemes.CustomScheme)
                .AddScheme<AuthenticationSchemeOptions, SampleAuthenticationHandler>(SampleAuthenticationSchemes.CustomScheme, o => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(SamplePolicyNames.CustomPolicy, policy =>
                    policy.AddRequirements(new SampleRequirement()));

                options.AddPolicy(SamplePolicyNames.CustomLogin, policy =>
                    policy.AddRequirements(new SampleLoginRequirement()));

                options.AddPolicy(SamplePolicyNames.CustomPolicyWithCustomForbiddenMessage, policy =>
                    policy.AddRequirements(new SampleWithCustomMessageRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, SampleRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, SampleLoginRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, SampleWithCustomMessageRequirementHandler>();
            services.AddTransient<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();

            // Add filtering globally, [USE THIS IF YOU WANT FILTERING APPLIED GLOBALLY, FORGET ABOVE]
            //services.AddControllers(options => options.Filters.Add(new HttpActionFilter()));
            //services.AddControllers(options => options.Filters.Add(new HttpExceptionFilter()));
            //services.AddControllers(options => options.Filters.Add(new HttpAuthroizationFilter()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiNetCore5", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiNetCore5 v1"));
            }

            app.UseExceptionHandler("/error");  // TODO: Need figure how this works.
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
