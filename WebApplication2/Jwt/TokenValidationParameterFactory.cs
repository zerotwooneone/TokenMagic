using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Url;

namespace WebApplication2.Jwt
{
    public class TokenValidationParameterFactory
    {
        private readonly ISecurityKeyRepository _securityKeyRepository;

        public TokenValidationParameterFactory(ISecurityKeyRepository securityKeyRepository)
        {
            _securityKeyRepository = securityKeyRepository;
        }

        public static void SetupUrlConfig(TokenValidationParameters tokenValidationParameters, UrlConfig urlConfig)
        {
            tokenValidationParameters.ValidAudiences = urlConfig.ValidAudiences;
            tokenValidationParameters.ValidIssuers = urlConfig.ValidIssuers;
        }

        public virtual TokenValidationParameters GetDefaulTokenValidationParameters()
        {
            var defaulTokenValidationParameters = new TokenValidationParameters();
            defaulTokenValidationParameters.IssuerSigningKeyResolver = IssuerSigningKeyResolver;
            
            return defaulTokenValidationParameters;
        }

        private IEnumerable<SecurityKey> IssuerSigningKeyResolver(string token, SecurityToken securitytoken, string kid, TokenValidationParameters validationparameters)
        {
            _securityKeyRepository.TryGet(kid, out var securityKey);
            return new[] {securityKey};
        }
    }
}