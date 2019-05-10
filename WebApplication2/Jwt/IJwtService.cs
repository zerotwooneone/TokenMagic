using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface IJwtService
    {
        bool IsValid(string token, 
            TokenValidationParameters tokenValidationParameters,
            out ClaimsPrincipal claimsPrincipal, 
            out SecurityToken securityToken,
            out SecurityTokenException securityTokenException,
            Action<JwtSecurityTokenHandler> handlerSetup = null);
        string CreateToken(SecurityTokenDescriptor securityTokenDescriptor,
            Action<JwtSecurityTokenHandler> handlerSetup = null);
    }

    public static class JwtServiceExtensions
    {
        public static bool IsValid(this IJwtService jwtService,
            string token,
            TokenValidationParameters tokenValidationParameters,
            out ClaimsPrincipal claimsPrincipal,
            Action<JwtSecurityTokenHandler> handlerSetup = null)
        {
            return jwtService.IsValid(token, tokenValidationParameters, out claimsPrincipal, out var securityToken,
                out var securityTokenException, handlerSetup);
        }
    }
}