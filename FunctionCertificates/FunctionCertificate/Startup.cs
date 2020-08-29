using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FunctionCertificate
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // ONLY ACCEPT CERTIFICATE WITH FOLLOWING thumbprint
            // 723A4D916F008B8464E1D314C6FABC1CB1E926BD
        }
    }
}
