using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FunctionCertificate
{
    public static class CertificateHelper
    {
        public static bool IsValidChainedCertificate(X509Certificate2 clientCertificate)
        {
            var serverCertificate = GetCertificate("182BC671E189654A66A0596A5EBADAFC6430B67D");
            X509Chain x509Chain = new X509Chain();

            var chain = x509Chain.Build(new X509Certificate2(clientCertificate));
            // Validate chain if using a trusted certificate

            return IsInChain(x509Chain, serverCertificate);
        }

        public static bool IsInChain(X509Chain clientX509Chain, X509Certificate2 serverCertificate)
        {
            X509Chain serverX509Chain = new X509Chain();
            serverX509Chain.Build(new X509Certificate2(serverCertificate));
            var rootThumbprintServer = serverX509Chain.ChainElements[serverX509Chain.ChainElements.Count - 1].Certificate.Thumbprint;

            for (int i = 0; i < clientX509Chain.ChainElements.Count; i++)
            {
                if (clientX509Chain.ChainElements[i].Certificate.Thumbprint == rootThumbprintServer)
                {
                    return true;
                }
            }

            return false;
        }

        public static  X509Certificate2 GetCertificate(string certificateThumbprint)
        {
            X509Certificate2 cert = null;

            // dev, test, production
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, false);
                if (certs.Count > 0)
                {
                    cert = certs[0];
                }
                store.Close();
            }

            // for local development
            if (cert == null)
            {
                cert = new X509Certificate2("serverl3.pfx", "1234");
            }

            return cert;
        }
    }
}
