using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureCertAuthClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get data!");
            var result = CallApi().GetAwaiter().GetResult();

            Console.WriteLine($"Success! {result}");
        }

        private static async Task<string> CallApi()
        {
            //var azureRandomStringBasicUrl = "https://functioncertificate20200830225033.azurewebsites.net/api/randomStringBasic";
            //return await CallAzureDeployedAPI(azureRandomStringBasicUrl);

            var azureRandomStringChainedUrl = "https://functioncertificate20200830225033.azurewebsites.net/api/randomStringChained";
            return await CallAzureDeployedAPI(azureRandomStringChainedUrl);

            //var localRandomStringBasicUrl = "http://localhost:7071/api/RandomStringBasic";
            //return await CallApiXARRClientCertHeader(localRandomStringBasicUrl);

            //var localRandomStringChainedUrl = "http://localhost:7071/api/RandomStringChained";
            //return await CallApiXARRClientCertHeader(localRandomStringChainedUrl);
        }

        // Test Azure deployment
        private static async Task<string> CallAzureDeployedAPI(string url)
        {
            var cert = new X509Certificate2("clientl3.pfx", "1234");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
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
        private static async Task<string> CallApiXARRClientCertHeader(string url)
        {
            try
            {
                var cert = new X509Certificate2("clientl3.pfx", "1234");
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(cert);
                var client = new HttpClient(handler);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };

                request.Headers.Add("X-ARR-ClientCert", Convert.ToBase64String(cert.RawData));
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
