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

            //var json2 = CallApiSelfSignedWithXARRClientCertHeader().GetAwaiter().GetResult();
            var json = CallAzureDeployedAPI().GetAwaiter().GetResult();
            
            Console.WriteLine("Success!");
        }

        // Test Azure deployment
        private static async Task<string> CallAzureDeployedAPI()
        {
            var cert = new X509Certificate2("clientOk.pfx", "1234");
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            var client = new HttpClient(handler);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://functioncertificate20200829221633.azurewebsites.net/api/randomString"),
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
        private static async Task<string> CallApiSelfSignedWithXARRClientCertHeader()
        {
            try
            {
                var cert = new X509Certificate2("clientOk.pfx", "1234");
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(cert);
                var client = new HttpClient(handler);

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("http://localhost:7071/api/RandomString"),
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
