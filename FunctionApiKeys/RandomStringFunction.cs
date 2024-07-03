using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace FunctionApiKeys;

public class RandomStringFunction
{
    private readonly ILogger<RandomStringFunction> _logger;

    public RandomStringFunction(ILogger<RandomStringFunction> logger)
    {
        _logger = logger;
    }

    [Function("RandomStringAuthLevelAnonymous")]
    public IActionResult RandomStringAuthLevelAnonymous(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

        return new OkObjectResult(GetEncodedRandomString());
    }

    [Function("RandomStringAuthLevelFunc")]
    public IActionResult RandomStringAuthLevelFunc(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger RandomStringAuthLevelFunc processed a request.");

        return new OkObjectResult(GetEncodedRandomString());
    }

    [Function("RandomStringAuthLevelAdmin")]
    public IActionResult RandomStringAuthLevelAdmin(
       [HttpTrigger(AuthorizationLevel.Admin, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger RandomStringAuthLevelAdmin processed a request.");

        return new OkObjectResult(GetEncodedRandomString());
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
