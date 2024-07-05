using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace FunctionNeworkSecurity;

public class RandomStringFunction
{
    private readonly ILogger<RandomStringFunction> _logger;
    private readonly MyConfigurationSecrets _myConfigurationSecrets;

    public RandomStringFunction(ILoggerFactory loggerFactory,
        IOptions<MyConfigurationSecrets> myConfigurationSecrets)
    {
        _logger = loggerFactory.CreateLogger<RandomStringFunction>();
        _myConfigurationSecrets = myConfigurationSecrets.Value;
    }

    [Function("RandomString")]
    public IActionResult RandomString(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger RandomStringAuthLevelAnonymous processed a request.");

        return new OkObjectResult($"{_myConfigurationSecrets.MySecret}  {GetEncodedRandomString()}");
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
