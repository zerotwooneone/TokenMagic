using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Url;

namespace WebApplication2.Jwt
{
    public class SecurityTokenDescriptorFactory
    {
        private readonly ISigningCredentialRepository _signingCredentialRepository;

        public SecurityTokenDescriptorFactory(ISigningCredentialRepository signingCredentialRepository)
        {
            _signingCredentialRepository = signingCredentialRepository;
        }

        public virtual SecurityTokenDescriptor GetDefaultSecurityTokenDescriptor()
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor();

            _signingCredentialRepository.TryGet("a4c3c6297910143307cd26a4704f0ec6ff7b75b1", out var signingCredentials);

            securityTokenDescriptor.SigningCredentials = signingCredentials;

            var subject = new ClaimsIdentity(new[] { new Claim("type", "value"), });
            securityTokenDescriptor.Subject = subject;

            DateTime? notBefore = null;
            DateTime? expires = DateTime.Now.AddYears(10);
            DateTime? issuedAt = null;

            securityTokenDescriptor.NotBefore = notBefore;
            securityTokenDescriptor.Expires = expires;
            securityTokenDescriptor.IssuedAt = issuedAt;

            return securityTokenDescriptor;
        }

        public virtual SecurityTokenDescriptor GetApplicationTokenDescriptor()
        {
            var securityTokenDescriptor = new SecurityTokenDescriptor();

            _signingCredentialRepository.TryGet("a4c3c6297910143307cd26a4704f0ec6ff7b75b1", out var signingCredentials);

            securityTokenDescriptor.SigningCredentials = signingCredentials;

            var subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "nothing"), });
            securityTokenDescriptor.Subject = subject;

            return securityTokenDescriptor;
        }

        public static void SetupUrlConfig(SecurityTokenDescriptor securityTokenDescriptor, UrlConfig urlConfig)
        {
            securityTokenDescriptor.Issuer = urlConfig.ValidIssuers.First();
            securityTokenDescriptor.Audience = urlConfig.ValidAudiences.First();
        }

    }
}