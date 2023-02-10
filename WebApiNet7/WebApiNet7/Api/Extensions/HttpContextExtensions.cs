
using Microsoft.IdentityModel.JsonWebTokens;

namespace WebApiNet7.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static long GetUserId(this IHttpContextAccessor httpContext)
        {
            var claim = httpContext.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            return claim == null || string.IsNullOrEmpty(claim.Value) ? 0L : long.Parse(claim.Value);
        }
    }
}
