using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    [Obsolete("This is meant for development only")]
    public class DummyX509Repository : ISigningCredentialRepository, ISecurityKeyRepository
    {
        private static readonly string Thumbprint = SanitizeThumbPrint("a4c3c6297910143307cd26a4704f0ec6ff7b75b1");

        public bool TryGet(string id, out SigningCredentials signingCredentials)
        {
            var cert = GetCertificate(Thumbprint);

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
            var cert = GetCertificate(Thumbprint);
            securityKey = new X509SecurityKey(cert);
            return true;
        }
    }
}