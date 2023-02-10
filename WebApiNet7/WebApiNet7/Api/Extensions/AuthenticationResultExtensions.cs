using Microsoft.AspNetCore.Identity;
using WebApiNet7.Api.Modules.Authentication.Models;

namespace WebApiNet7.Api.Extensions
{
    public static class AuthenticationResultExtensions
    {
        public static AuthenticationResult WithError(this AuthenticationResult authenticationResult, string code, string description)
        {
            authenticationResult.Errors?.Add(new IdentityError
            {
                Code = code,
                Description = description,
            });

            return authenticationResult;
        }
    }
}
