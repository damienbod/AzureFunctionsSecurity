using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace FunctionIdentityUserAccess;

public class RandomStringFunction
{
    private readonly ILogger _log;
    private readonly AzureADJwtBearerValidation _azureADJwtBearerValidation;

    public RandomStringFunction(ILoggerFactory loggerFactory,
        AzureADJwtBearerValidation azureADJwtBearerValidation)
    {
        _log = loggerFactory.CreateLogger<RandomStringFunction>(); ;
        _azureADJwtBearerValidation = azureADJwtBearerValidation;
    }

    [FunctionName("RandomString")]
    public async Task<IActionResult> RandomString(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        try
        {
            _log.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

            ClaimsPrincipal principal; // This can be used for any claims
            if ((principal = await _azureADJwtBearerValidation.ValidateTokenAsync(req.Headers["Authorization"])) == null)
            {
                return new UnauthorizedResult();
            }

            var claimsName = $"Bearer token claim preferred_username: {_azureADJwtBearerValidation.GetPreferredUserName()}";
            return new OkObjectResult($"{claimsName} {GetEncodedRandomString()}");
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
