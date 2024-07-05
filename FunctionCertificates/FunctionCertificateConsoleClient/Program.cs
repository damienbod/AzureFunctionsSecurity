using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AzureCertAuthClientConsole;

class Program
{
    async static Task Main(string[] args)
    {
        Console.WriteLine("Let's try to get a random string from the Azure Function using a certificate!");
        Console.WriteLine("----");
        var result = await CallApi();
        Console.WriteLine($"{result}");
        Console.WriteLine("----");
        Console.WriteLine($"Success!");
    }

    private static async Task<string> CallApi()
    {
        var cert = new X509Certificate2("functionsCertAuth.pfx", "1234");
        var azureRandomStringBasicUrl = "https://functioncertificate20240704202458.azurewebsites.net/api/RandomStringCertAuth";
        return await CallApiClientCertHeader(azureRandomStringBasicUrl, cert, false);

        //var cert = new X509Certificate2("client401.pfx", "1234");
        //var localRandomStringBasicUrl = "http://localhost:7108/api/RandomStringCertAuth";
        //return await CallApiClientCertHeader(localRandomStringBasicUrl, cert, true);
    }

    private static async Task<string> CallApiClientCertHeader(string url, X509Certificate2 clientCertificate, bool isLocalTesting)
    {
        try
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(clientCertificate);
            var client = new HttpClient(handler);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            // Only needed for testing on local host
            if (isLocalTesting)
            {
                request.Headers.Add("X-ARR-ClientCert", Convert.ToBase64String(clientCertificate.RawData));
            }
            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                return responseContent;
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Exception {e}");
        }
    }
}
