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
        private readonly CertificateAuthService _certificateHelper;

        public RandomStringFunction(CertificateAuthService certificateHelper, ILoggerFactory loggerFactory)
        {
            _certificateHelper = certificateHelper;
            _log = loggerFactory.CreateLogger<RandomStringFunction>();
        }

        [FunctionName("RandomStringCertAuth")]
        public IActionResult RandomStringCertAuth(
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
                // https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth#aspnet-sample

                // Read from an APP.SETTING so this can be chaged without a new deployment
                if (clientCert.Thumbprint != "5726F1DDBC5BA5986A21BDFCBA1D88C74C8EDE90")
                {
                    return new BadRequestObjectResult("A valid client certificate is not used");
                }

                if (DateTime.Compare(DateTime.UtcNow, clientCert.NotBefore) < 0
                            || DateTime.Compare(DateTime.UtcNow, clientCert.NotAfter) > 0)
                {
                    return new BadRequestObjectResult("client certificate not in alllowed time interval");
                }

                return new OkObjectResult(GetEncodedRandomString());
            }

            return new BadRequestObjectResult("A valid client certificate is not found");            
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
