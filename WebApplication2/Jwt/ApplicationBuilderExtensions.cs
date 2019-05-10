using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.Url;

namespace WebApplication2.Jwt
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder EnsureApplicationToken(this IApplicationBuilder applicationBuilder)
        {
            var serviceProvider = applicationBuilder.ApplicationServices;

            var appConfigService = serviceProvider.GetRequiredService<AppConfigService>();
            var appConfig = appConfigService.Get();
            if (appConfig != null) return applicationBuilder;
            
            var urlConfig = serviceProvider.GetRequiredService<UrlConfig>();
            var jwtService= serviceProvider.GetRequiredService<IJwtService>();
            var securityTokenDescriptorFactory= serviceProvider.GetRequiredService<SecurityTokenDescriptorFactory>();

            void HandlerSetup(JwtSecurityTokenHandler jwtSecurityTokenHandler)
            {
                jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = false;
            }

            var securityTokenDescriptor = securityTokenDescriptorFactory.GetApplicationTokenDescriptor();

            SecurityTokenDescriptorFactory.SetupUrlConfig(securityTokenDescriptor, urlConfig);

            var encoded =jwtService.CreateToken(securityTokenDescriptor, HandlerSetup);

            var newAppConfig = new AppConfig
            {
                AppToken = encoded
            };
            
            appConfigService.Set(newAppConfig);
            
            return applicationBuilder;
        }
    }
}