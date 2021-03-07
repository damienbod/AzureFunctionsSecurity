using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureCertAuthClientConsole
{
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
            var cert = new X509Certificate2("client401.pfx", "1234");

            var azureRandomStringBasicUrl = "https://functioncertificate20200830225033.azurewebsites.net/api/RandomStringCertAuth";
            return await CallAzureDeployedAPI(azureRandomStringBasicUrl, cert);

            //var localRandomStringBasicUrl = "http://localhost:7071/api/RandomStringCertAuth";
            //return await CallApiXARRClientCertHeader(localRandomStringBasicUrl, cert);

        }

        // Test Azure deployment
        private static async Task<string> CallAzureDeployedAPI(string url, X509Certificate2 clientCertificate)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(clientCertificate);
            var client = new HttpClient(handler);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return responseContent;
            }

            throw new ApplicationException($"Status code: {response.StatusCode}, Error: {response.ReasonPhrase}");
        }

        // Local dev
        private static async Task<string> CallApiXARRClientCertHeader(string url, X509Certificate2 clientCertificate)
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

                request.Headers.Add("X-ARR-ClientCert", Convert.ToBase64String(clientCertificate.RawData));
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
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
}
