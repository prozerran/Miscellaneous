using Microsoft.AspNetCore.Identity;
using WebApiNet7.Api.Modules.Authentication.Models;

namespace WebApiNet7.Services
{
    public interface IUserAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateUser(string email, string password);
    }
}
