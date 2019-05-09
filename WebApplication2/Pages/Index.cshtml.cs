using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.System;
using WebApplication2.Url;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UrlConfig _urlConfig;

        public IndexModel(UrlConfig urlConfig)
        {
            _urlConfig = urlConfig;
        }
        public string TokenString { get; private set; }
        public void OnGet()
        {
            var xs = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            const string rawThumbprint = "a4c3c6297910143307cd26a4704f0ec6ff7b75b1";
            string thumbprint = SanitizeThumbPrint(rawThumbprint);
            X509Certificate2 cert;
            try
            {
                xs.Open(OpenFlags.ReadOnly);
                var certCollection = xs.Certificates;
                var signingCert = certCollection.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (signingCert.Count == 0)
                {
                    throw new Exception($"Cert with thumbprint: '{thumbprint}' not found in local machine cert store.");
                }
                cert = signingCert[0];
            }
            finally
            {
                xs.Close();
            }

            var s = new JwtSecurityTokenHandler();
            var x509SigningCredentials = new X509SigningCredentials(cert);
            
            var subject = new ClaimsIdentity(new[] { new Claim("type", "value"), });
            string tokenAuthority = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var audience = tokenAuthority;
            var issuer = tokenAuthority;
            DateTime? notBefore = null;
            DateTime? expires = DateTime.Now.AddYears(10).ToUniversalTime();
            DateTime? issuedAt = null;

            var tokenDesc = new SecurityTokenDescriptor
            {
                Audience = audience,
                EncryptingCredentials = null, 
                Expires = expires,
                IssuedAt = issuedAt,
                Issuer = issuer,
                NotBefore = notBefore,
                SigningCredentials = x509SigningCredentials,
                Subject = subject
            };

            var encoded =
                s.CreateEncodedJwt(tokenDesc);
            var chunks = encoded.Chunk(_urlConfig.MaxUrlChunkSize);
            TokenString  = string.Join("/", chunks) ;
        }

        public string SanitizeThumbPrint(string thumbprint)
        {
            return Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
        }
    }
}