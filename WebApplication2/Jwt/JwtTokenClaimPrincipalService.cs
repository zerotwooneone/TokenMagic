using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public class JwtTokenClaimPrincipalService : ITokenClaimPrincipalService
    {
        private readonly ISigningKeyRepository _signingKeyRepository;
        private readonly IJwtService _jwtService;
        private readonly ISigningCredentialRepository _signingCredentialRepository;

        public JwtTokenClaimPrincipalService(ISigningKeyRepository signingKeyRepository,
            IJwtService jwtService,
            ISigningCredentialRepository signingCredentialRepository)
        {
            _signingKeyRepository = signingKeyRepository;
            _jwtService = jwtService;
            _signingCredentialRepository = signingCredentialRepository;
        }
        public bool TryGetClaimsPrincipal(string token, 
            string signingKeyId, 
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

        public bool TryGetTokenString(string signingKeyId, 
            Action<SecurityTokenDescriptor> tokenSetup, out string token)
        {
            if (_signingCredentialRepository.TryGet(signingKeyId, out var signingCredentials))
            {
                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = signingCredentials,
                };
                tokenSetup(securityTokenDescriptor);
                token = _jwtService.CreateToken(securityTokenDescriptor);
                return true;
            }

            token = null;
            return false;
        }
    }
}