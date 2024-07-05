using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace FunctionIdentityUserAccess;

public class RandomStringFunction
{
    private readonly ILogger<RandomStringFunction> _logger;
    private readonly EntraIDJwtBearerValidation _meIdJwtBearerValidation;

    public RandomStringFunction(ILogger<RandomStringFunction> logger,
        EntraIDJwtBearerValidation meIdJwtBearerValidation)
    {
        _logger = logger;
        _meIdJwtBearerValidation = meIdJwtBearerValidation;
    }

    [Function("RandomString")]
    public async Task<IActionResult> RandomString([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        try
        {
            _logger.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

            var tokenValidationResult = await _meIdJwtBearerValidation.ValidateTokenAsync(req.Headers["Authorization"]);

            if (tokenValidationResult == null)
            {
                return new UnauthorizedResult();
            }

            var claimsName = $"Bearer token claim preferred_username: {_meIdJwtBearerValidation.GetPreferredUserName()}";

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
        return RandomNumberGenerator.GetBytes(length);
    }
}
