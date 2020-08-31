using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace FunctionCertificate
{
    public class CertificateAuthService
    {
        public bool IsValidCertificate(X509Certificate2 clientCertificate, ILogger log)
        {
            // read from a APP.SETTING is using this
            var serverCertificate = GetCertificate("5726F1DDBC5BA5986A21BDFCBA1D88C74C8EDE90", log);

            return IsValidClientCertificate(clientCertificate, serverCertificate, log);
        }

        private bool IsValidClientCertificate(X509Certificate2 clientCertificate, X509Certificate2 serverCertificate, ILogger log)
        {
            // check certificate is loaded on the server
            // can be removed or changed if the Thumbprint is read from the APP settings
            if (clientCertificate.Thumbprint != serverCertificate.Thumbprint)
            {
                return false;
            }

            if (DateTime.Compare(DateTime.UtcNow, clientCertificate.NotBefore) < 0
                || DateTime.Compare(DateTime.UtcNow, clientCertificate.NotAfter) > 0)
            {
                return false;
            }

            // Add whatever other checks makes sense

            return true;
        }

        /// <summary>
        /// WEBSITE_LOAD_CERTIFICATES APP.SETTING required in Azure to work
        /// </summary>
        /// <param name="certificateThumbprint"></param>
        /// <param name="log"></param>
        /// <returns></returns>
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
                cert = new X509Certificate2("functionsCertAuth.pfx", "1234");
            }

            if (cert == null)
            {
                log.LogError($"No certificate found...");
            }

            return cert;
        }
    }
}
