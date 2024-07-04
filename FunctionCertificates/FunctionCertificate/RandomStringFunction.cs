using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace FunctionCertificate;

public class RandomStringFunction
{
    private readonly ILogger<RandomStringFunction> _logger;

    public RandomStringFunction(ILogger<RandomStringFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Only the Thumbprint, NotBefore and NotAfter are checked, further validation of the client can/should be added
    /// Chained certificate do not work with Azure App services, X509Chain only loads the certificate, not the chain on Azure
    /// Maybe due to the chain being not trusted. (Works outside Azure)
    /// Certificate validation docs
    /// https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/Certificate/src/CertificateAuthenticationHandler.cs
    /// https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth#aspnet-sample
    /// </summary>
    [Function("RandomStringCertAuth")]
    public IActionResult RandomStringCertAuth([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] 
        HttpRequest request)
    {
        _logger.LogInformation("C# HTTP trigger RandomString processed a request.");

        var clientCert = request.HttpContext.Request.GetRequestContext().ClientCertificate;

        StringValues cert;
        if (request.Headers.TryGetValue("X-ARR-ClientCert", out cert))
        {
            if (cert[0] == null) throw new ArgumentNullException(nameof(cert));

            byte[] clientCertBytes = Convert.FromBase64String(cert[0]);
            var clientCert = new X509Certificate2(clientCertBytes);

            // Validate Thumbprint
            if (clientCert.Thumbprint != "5726F1DDBC5BA5986A21BDFCBA1D88C74C8EDE90")
            {
                return new BadRequestObjectResult($"A valid client certificate is not used: {clientCert.Thumbprint}");
            }

            // Validate NotBefore and NotAfter
            if (DateTime.Compare(DateTime.UtcNow, clientCert.NotBefore) < 0
                        || DateTime.Compare(DateTime.UtcNow, clientCert.NotAfter) > 0)
            {
                return new BadRequestObjectResult("client certificate not in alllowed time interval");
            }

            // Add further validation of certificate as required.

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
        return RandomNumberGenerator.GetBytes(length);
    }
}
