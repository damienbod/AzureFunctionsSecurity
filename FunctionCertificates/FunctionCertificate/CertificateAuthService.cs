using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace FunctionCertificate
{
    public class CertificateAuthService
    {
        public bool IsValidChainedCertificate(X509Certificate2 clientCertificate, ILogger log)
        {
            var serverCertificate = GetCertificate("182BC671E189654A66A0596A5EBADAFC6430B67D", log);

            return IsValidClientCertificate(clientCertificate, serverCertificate, log);
        }

        private bool IsValidClientCertificate(X509Certificate2 clientCertificate, X509Certificate2 serverCertificate, ILogger log)
        {
            throw new NotImplementedException();
        }

        private X509Certificate2 GetCertificate(string certificateThumbprint, ILogger log)
        {
            if (string.IsNullOrEmpty(certificateThumbprint))
            {
                throw new ArgumentNullException("no certificateThumbprint");
            }

            log.LogInformation($"looking for certificate with Thumbprint: {certificateThumbprint}");

            X509Certificate2 cert = null;
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
                if (certs.Count > 0)
                {
                    log.LogInformation($"Found certificate: {certs[0].Thumbprint}");
                    cert = certs[0];
                }
                store.Close();
            }

            // for local development
            if (cert == null)
            {
                log.LogWarning($"No certificate found with Thumbprint, try local debug cert");
                cert = new X509Certificate2("serverl3.pfx", "1234");
            }

            if (cert == null)
            {
                log.LogError($"No certificate found...");
            }
            return cert;
        }
    }
}
