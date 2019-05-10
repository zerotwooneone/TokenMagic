using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public class X509Repository : ISigningCredentialRepository, ISecurityKeyRepository
    {
        public bool TryGet(string id, out SigningCredentials signingCredentials)
        {
            var sanitized = SanitizeThumbPrint(id);
            var cert = GetCertificate(sanitized);

            var x509SigningCredentials = new X509SigningCredentials(cert);
            signingCredentials = x509SigningCredentials;
            return true;
        }

        private static X509Certificate2 GetCertificate(string thumbprint)
        {
            var xs = new X509Store(StoreName.My, StoreLocation.LocalMachine);
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

            return cert;
        }

        public static string SanitizeThumbPrint(string thumbprint)
        {
            return Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
        }

        public bool TryGet(string id, out SecurityKey securityKey)
        {
            var sanitized = SanitizeThumbPrint(id);
            var cert = GetCertificate(sanitized);
            securityKey = new X509SecurityKey(cert);
            return true;
        }
    }
}