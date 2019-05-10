using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Controllers;
using WebApplication2.Jwt;
using WebApplication2.System;
using WebApplication2.Url;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UrlConfig _urlConfig;
        private readonly IJwtService _jwtService;
        private readonly SecurityTokenDescriptorFactory _securityTokenDescriptorFactory;

        public IndexModel(UrlConfig urlConfig,
            IJwtService jwtService,
            SecurityTokenDescriptorFactory securityTokenDescriptorFactory)
        {
            _urlConfig = urlConfig;
            _jwtService = jwtService;
            _securityTokenDescriptorFactory = securityTokenDescriptorFactory;
        }
        public string TokenString { get; private set; }
        public void OnGet()
        {
            void HandlerSetup(JwtSecurityTokenHandler jwtSecurityTokenHandler)
            {
                jwtSecurityTokenHandler.SetDefaultTimesOnTokenCreation = true;
            }

            var securityTokenDescriptor = _securityTokenDescriptorFactory.GetDefaultSecurityTokenDescriptor();

            SecurityTokenDescriptorFactory.SetupUrlConfig(securityTokenDescriptor, _urlConfig);

            var encoded =_jwtService.CreateToken(securityTokenDescriptor, HandlerSetup);

            var chunks = encoded.Chunk(_urlConfig.MaxUrlChunkSize);
            TokenString  = string.Join("/", chunks) ;
        }
    }
}