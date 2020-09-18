using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunctionIdentityUserAccess
{
    public class RandomStringFunction
    {
        private readonly ILogger _log;
        private readonly MyConfigurationSecrets _myConfigurationSecrets;
        private readonly AuthJwtBearerValidation _authJwtValidation;

        public RandomStringFunction(ILoggerFactory loggerFactory,
            IOptions<MyConfigurationSecrets> myConfigurationSecrets,
            AuthJwtBearerValidation authJwtValidation)
        {
            _log = loggerFactory.CreateLogger<RandomStringFunction>();
            _myConfigurationSecrets = myConfigurationSecrets.Value;
            _authJwtValidation = authJwtValidation;
        }

        [FunctionName("RandomString")]
        public async Task<IActionResult> RandomString(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _log.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");
                ClaimsPrincipal principal;
                if ((principal = await _authJwtValidation.ValidateTokenAsync(req.Headers["Authorization"])) == null)
                {
                    return new UnauthorizedResult();
                }

                StringBuilder sb = new StringBuilder();
                foreach (var claim in principal.Claims)
                {
                    sb.AppendLine($"{claim.Type} {claim.Value}");
                }

                return new OkObjectResult($"{sb}  {GetEncodedRandomString()}");
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"{ex.Message}");
            }
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
