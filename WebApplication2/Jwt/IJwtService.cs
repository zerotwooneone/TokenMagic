using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface IJwtService
    {
        bool IsValid(string token, TokenValidationParameters tokenValidationParameters,out ClaimsPrincipal claimsPrincipal, out SecurityToken securityToken);
        string CreateToken(SecurityTokenDescriptor securityTokenDescriptor);
    }
}