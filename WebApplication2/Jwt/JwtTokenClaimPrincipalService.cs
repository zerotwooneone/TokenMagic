using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public class JwtTokenClaimPrincipalService : ITokenClaimPrincipalService
    {
        private readonly ISigningKeyRepository _signingKeyRepository;
        private readonly IJwtService _jwtService;

        public JwtTokenClaimPrincipalService(ISigningKeyRepository signingKeyRepository,
            IJwtService jwtService)
        {
            _signingKeyRepository = signingKeyRepository;
            _jwtService = jwtService;
        }
        public bool TryGetClaimsPrincipal(string token, string signingKeyId, 
            Action<TokenValidationParameters> tokenValidationSetup, 
            out ClaimsPrincipal claimsPrincipal)
        {
            if(_signingKeyRepository.TryGet(signingKeyId, out var signingCredentials))
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = signingCredentials
                };
                tokenValidationSetup(tokenValidationParameters);

                if (_jwtService.IsValid(token,
                    tokenValidationParameters,
                    out claimsPrincipal,
                    out var securityToken))
                {
                    return true;
                }
            }

            claimsPrincipal = null;
            return false;
        }
    }
}