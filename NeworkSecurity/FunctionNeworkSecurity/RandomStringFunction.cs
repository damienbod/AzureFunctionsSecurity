using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;

namespace FunctionNeworkSecurity
{
    public class RandomStringFunction
    {
        private readonly ILogger _log;
        private readonly MyConfigurationSecrets _myConfigurationSecrets;

        public RandomStringFunction(ILoggerFactory loggerFactory,
            IOptions<MyConfigurationSecrets> myConfigurationSecrets)
        {
            _log = loggerFactory.CreateLogger<RandomStringFunction>();
            _myConfigurationSecrets = myConfigurationSecrets.Value;
        }

        [FunctionName("RandomString")]
        public IActionResult RandomString(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

            return new OkObjectResult($"{_myConfigurationSecrets.MySecret}  {GetEncodedRandomString()}");
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
