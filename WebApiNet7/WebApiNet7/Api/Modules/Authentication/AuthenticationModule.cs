using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApis.Extensions.Binding;
using System.Reflection;
using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Modules.Authentication.Models;
using WebApiNet7.Services;

namespace WebApiNet7.Api.Modules.Authentication
{
    public sealed class AuthenticationModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services) => services;

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            #region Commands

            endpoints.MapPost("/api/authentication", async Task<Results<ValidationProblem, Ok<AuthenticatedUser>, UnauthorizedHttpResult>> (Validated<NewSignIn> request,
                    IUserAuthenticationService userAuthenticationService,
                    IMapper mapper) =>
                {
                    if (!request.IsValid || request.Value == null)
                        return TypedResults.ValidationProblem(request.Errors);

                    var authenticationResult = await userAuthenticationService.AuthenticateUser(request.Value.Email, request.Value.Password);

                    return authenticationResult.IsSuccess
                        ? TypedResults.Ok(mapper.Map<AuthenticatedUser>(authenticationResult))
                        : TypedResults.Unauthorized();
                })
                .WithName("SignIn")
                .WithTags("Authentication")
                .Accepts<NewSignIn>("application/json");

            #endregion

            return endpoints;
        }
    }
}
