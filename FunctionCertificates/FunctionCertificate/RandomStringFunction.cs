using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.Extensions.Primitives;

namespace FunctionCertificate
{
    public class RandomStringFunction
    {
        private readonly ILogger _log;

        public RandomStringFunction(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<RandomStringFunction>();
        }

        [FunctionName("RandomStringBasic")]
        public IActionResult RandomStringBasic(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomString processed a request.");

            StringValues cert;
            if (req.Headers.TryGetValue("X-ARR-ClientCert", out cert ))
            {
                byte[] clientCertBytes = Convert.FromBase64String(cert[0]);
                X509Certificate2 clientCert = new X509Certificate2(clientCertBytes);

                // only the Thumbprint is checked, further validation of the client can/should be added
                // example certificate validation
                // https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/Certificate/src/CertificateAuthenticationHandler.cs
                // you could load a root cert to the Azure app Service and validate the chain etc
                if (clientCert.Thumbprint == "723A4D916F008B8464E1D314C6FABC1CB1E926BD")
                {
                    return new OkObjectResult(GetEncodedRandomString());
                }
            }

            return new UnauthorizedObjectResult("A valid client certificate is not found");            
        }

        [FunctionName("RandomStringChained")]
        public IActionResult RandomStringChained(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomString processed a request.");

            StringValues cert;
            if (req.Headers.TryGetValue("X-ARR-ClientCert", out cert))
            {
                byte[] clientCertBytes = Convert.FromBase64String(cert[0]);
                X509Certificate2 clientCert = new X509Certificate2(clientCertBytes);
                if(IsValidChainedCertificate(clientCert))
                {
                    return new OkObjectResult(GetEncodedRandomString());
                }
            }

            return new UnauthorizedObjectResult("A valid client certificate is not found");
        }
        

        private bool IsValidChainedCertificate(X509Certificate2 clientCertificate)
        {
            var serverCertificate = new X509Certificate2("localhost_intermediate_l2.pfx", "1234");
            X509Chain x509Chain = new X509Chain();
            var chain = x509Chain.Build(new X509Certificate2(clientCertificate));
            var test = x509Chain.ChainElements[x509Chain.ChainElements.Count - 1].Certificate.Thumbprint == serverCertificate.Thumbprint;
            
            if (x509Chain.Build(new X509Certificate2(clientCertificate)))
            {
                return x509Chain.ChainElements[x509Chain.ChainElements.Count - 1]
                    .Certificate.Thumbprint == serverCertificate.Thumbprint; 
            }

            //if (clientCertificate.Thumbprint == "723A4D916F008B8464E1D314C6FABC1CB1E926BD")
            //{
                
            //    return true;
            //}
            return false;
        }

        private string GetEncodedRandomString()
        {
            var base64 = Convert.ToBase64String(GenerateRandomBytes(100));
            return HtmlEncoder.Default.Encode(base64);
        }

        private byte[] GenerateRandomBytes(int length)
        {
            using var randonNumberGen = new RNGCryptoServiceProvider();
            var byteArray = new byte[length];
            randonNumberGen.GetBytes(byteArray);
            return byteArray;
        }
    }
}
