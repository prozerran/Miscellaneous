using Microsoft.AspNetCore.Identity;

namespace WebApiNet7.Api.Modules.Authentication.Models
{
    // SHOULD BELONG IN COMMON

    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public string? User { get; set; }   // should be user of : IdentityUser<long>
        public string? AuthToken { get; set; }
        public List<IdentityError>? Errors { get; set; } = new();
    }
}
