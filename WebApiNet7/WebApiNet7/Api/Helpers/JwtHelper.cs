using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiNet7.Api.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtForUser(long userId,
            string issuer,
            string audience,
            string securityKey,
            IEnumerable<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString(), valueType: ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Iss, issuer),
            new Claim(JwtRegisteredClaimNames.Aud, audience),
            new Claim(type: JwtRegisteredClaimNames.Nbf,
                value: new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString(),
                valueType: ClaimValueTypes.Integer64),
            new Claim(type: JwtRegisteredClaimNames.Exp,
                value: new DateTimeOffset(DateTime.Now.AddDays(180)).ToUnixTimeSeconds().ToString(),
                valueType: ClaimValueTypes.Integer64),
        };

            claims.AddRange(roles.Select(role => new Claim("role", role)));

            var jwtPayload = new JwtPayload(claims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtHeader = new JwtHeader(signingCredentials);
            var jwtSecurityToken = new JwtSecurityToken(jwtHeader, jwtPayload);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
