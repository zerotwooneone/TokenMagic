using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface ITokenClaimPrincipalService
    {
        bool TryGetClaimsPrincipal(string token, 
            string signingKeyId, 
            Action<TokenValidationParameters> tokenValidationSetup, 
            out ClaimsPrincipal claimsPrincipal);
    }
}