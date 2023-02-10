using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiNet7.Models;

namespace WebApiNet7.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection AddUserJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
        {
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Custom";
            });

            // Stop the JWT bearer middleware converting the JWT claim types into the antiquated SOAP claim types
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            _ = jwtOptions ?? throw new ArgumentException("JwtOptions configuration was not found");

            authenticationBuilder
                .AddPolicyScheme("Custom", "Custom", options =>
                {
                    options.ForwardDefaultSelector = context => context.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCulture)
                        ? JwtBearerDefaults.AuthenticationScheme
                        : IdentityConstants.ApplicationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidAlgorithms = new[] { "HS256" },

                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true, // Validate the expiration and not before values in the token
                        RequireExpirationTime = true,

                        ClockSkew = TimeSpan.Zero // No tolerance for the expiration date
                    };
                });

            return services;
        }

        public static IServiceCollection AddUserAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(PolicyNames.AdminsOnly, p
            //        => p.RequireClaim("role", RoleNames.Admin));
            //});

            return services;
        }
    }
}
