using System;
using System.Linq;
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
        private readonly ITokenClaimPrincipalService _tokenClaimPrincipalService;

        public IndexModel(UrlConfig urlConfig,
            ITokenClaimPrincipalService tokenClaimPrincipalService)
        {
            _urlConfig = urlConfig;
            _tokenClaimPrincipalService = tokenClaimPrincipalService;
        }
        public string TokenString { get; private set; }
        public void OnGet()
        {
            _tokenClaimPrincipalService.TryGetTokenString(FirstController.SigningKeyId, TokenSetup, out var encoded);

            var chunks = encoded.Chunk(_urlConfig.MaxUrlChunkSize);
            TokenString  = string.Join("/", chunks) ;
        }

        private void TokenSetup(SecurityTokenDescriptor securityTokenDescriptor)
        {
            var subject = new ClaimsIdentity(new[] { new Claim("type", "value"), });
            securityTokenDescriptor.Subject = subject;

            securityTokenDescriptor.Issuer = _urlConfig.ValidIssuers.First();
            securityTokenDescriptor.Audience = _urlConfig.ValidAudiences.First();

            DateTime? notBefore = null;
            DateTime? expires = DateTime.Now.AddYears(10).ToUniversalTime();
            DateTime? issuedAt = null;

            securityTokenDescriptor.NotBefore = notBefore;
            securityTokenDescriptor.Expires = expires;
            securityTokenDescriptor.IssuedAt = issuedAt;
        }
    }
}