using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApiNet7.Api.Extensions;
using WebApiNet7.Api.Helpers;
using WebApiNet7.Api.Modules.Authentication.Models;
using WebApiNet7.Models;

namespace WebApiNet7.Services
{
    public sealed class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly ILogger<UserAuthenticationService> _logger;
        private readonly JwtOptions _jwtOptions;

        public UserAuthenticationService(
            ILogger<UserAuthenticationService> logger,
            IOptions<JwtOptions> jwtOptions)
        {
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<AuthenticationResult> AuthenticateUser(string email, string password)
        {
            var authenticationResult = new AuthenticationResult();

            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return authenticationResult.WithError("invalid-login", "Invalid email or password");

                // TODO: actually validate and check user/pass
                await Task.Delay(10);

                var roles = new List<string> { "admin" };

                authenticationResult.AuthToken = JwtHelper.GenerateJwtForUser(userId: 1,
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    securityKey: _jwtOptions.Key,
                    roles: roles);

                authenticationResult.User = "User";
                authenticationResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{RequestName} failed", nameof(AuthenticateUser));
                authenticationResult.WithError("internal-error", ex.Message);
            }
            return authenticationResult;
        }
    }
}
