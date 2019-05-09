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
            var jwtSecurityTokenHandler = new Lazy<JwtSecurityTokenHandler>(() =>
            {
                return new JwtSecurityTokenHandler();
            });
            serviceCollection
                .TryAddSingleton<IJwtService>(sp =>
                {
                    return new JwtTokenHandlerWrapper(jwtSecurityTokenHandler.Value);
                });
            serviceCollection
                .TryAddSingleton<ISigningCredentialRepository, DummySigningCredentialRepository>();
            serviceCollection
                .TryAddSingleton<ISigningKeyRepository, DummySigningCredentialRepository>();
            serviceCollection
                .TryAddSingleton<ITokenClaimPrincipalService, JwtTokenClaimPrincipalService>();
            
            return serviceCollection;
        }
    }
}