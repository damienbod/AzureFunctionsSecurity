using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace FunctionApiKeys
{
    public class RandomStringFunction
    {
        private readonly ILogger _log;

        public RandomStringFunction(ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<RandomStringFunction>();
        }

        [FunctionName("RandomStringAuthLevelAnonymous")]
        public IActionResult RandomStringAuthLevelAnonymous(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

            return new OkObjectResult(GetEncodedRandomString());
        }

        [FunctionName("RandomStringAuthLevelFunc")]
        public IActionResult RandomStringAuthLevelFunc(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomStringAuthLevelFunc processed a request.");

            return new OkObjectResult(GetEncodedRandomString());
        }

        [FunctionName("RandomStringAuthLevelAdmin")]
        public IActionResult RandomStringAuthLevelAdmin(
           [HttpTrigger(AuthorizationLevel.Admin, "get", Route = null)] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger RandomStringAuthLevelAdmin processed a request.");

            return new OkObjectResult(GetEncodedRandomString());
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
