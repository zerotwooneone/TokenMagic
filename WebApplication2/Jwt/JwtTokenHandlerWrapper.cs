using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public class JwtTokenHandlerWrapper: IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtTokenHandlerWrapper(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public bool IsValid(string token, TokenValidationParameters tokenValidationParameters, out ClaimsPrincipal claimsPrincipal, out SecurityToken securityToken)
        {
            try
            {
                claimsPrincipal =
                    _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                return true;
            }
            catch (SecurityTokenException securityTokenException)
            {
                claimsPrincipal = null;
                securityToken = null;
                return false;
            }
        }

        public string CreateToken(SecurityTokenDescriptor securityTokenDescriptor)
        {
            return _jwtSecurityTokenHandler.CreateEncodedJwt(securityTokenDescriptor);
        }
    }
}