using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebApplication2.Jwt
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddJwt(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .TryAddSingleton<IJwtService>(sp =>
                {
                    JwtSecurityTokenHandler JwtSecurityTokenHandlerFactory()
                    {
                        return new JwtSecurityTokenHandler();
                    }

                    return new JwtTokenHandlerWrapper(JwtSecurityTokenHandlerFactory);
                });
            serviceCollection
                .TryAddSingleton<ISigningCredentialRepository, X509Repository>();
            serviceCollection
                .TryAddSingleton<ISecurityKeyRepository, X509Repository>();
            serviceCollection
                .TryAddSingleton<TokenValidationParameterFactory>();
            serviceCollection
                .TryAddSingleton<SecurityTokenDescriptorFactory>();
            
            return serviceCollection;
        }
    }
}