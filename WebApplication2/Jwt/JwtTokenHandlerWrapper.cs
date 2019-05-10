using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public class JwtTokenHandlerWrapper: IJwtService
    {
        private readonly Func<JwtSecurityTokenHandler> _jwtSecurityTokenHandlerFactory;

        public JwtTokenHandlerWrapper(Func<JwtSecurityTokenHandler> jwtSecurityTokenHandlerFactory)
        {
            _jwtSecurityTokenHandlerFactory = jwtSecurityTokenHandlerFactory;
        }

        public bool IsValid(string token, 
            TokenValidationParameters tokenValidationParameters, 
            out ClaimsPrincipal claimsPrincipal, 
            out SecurityToken securityToken,
            out SecurityTokenException securityTokenException,
            Action<JwtSecurityTokenHandler> handlerSetup = null)
        {
            var jwtSecurityTokenHandler = _jwtSecurityTokenHandlerFactory();
            handlerSetup?.Invoke(jwtSecurityTokenHandler);
            try
            {
                claimsPrincipal =
                    jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                securityTokenException = null;
                return true;
            }
            catch (SecurityTokenException ste)
            {
                claimsPrincipal = null;
                securityToken = null;
                securityTokenException = ste;
                return false;
            }
        }

        public string CreateToken(SecurityTokenDescriptor securityTokenDescriptor,
            Action<JwtSecurityTokenHandler> handlerSetup = null)
        {
            var jwtSecurityTokenHandler = _jwtSecurityTokenHandlerFactory();
            handlerSetup?.Invoke(jwtSecurityTokenHandler);
            return jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptor);
        }
    }
}