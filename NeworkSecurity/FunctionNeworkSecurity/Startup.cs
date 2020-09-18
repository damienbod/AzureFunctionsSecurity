using FunctionNeworkSecurity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionNeworkSecurity
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<MyConfigurationSecrets>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("MyConfigurationSecrets").Bind(settings);
                });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                // using Key Vault, either local dev or deployed
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                builder.ConfigurationBuilder
                    .AddAzureKeyVault(keyVaultEndpoint)
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", true)
                    .AddEnvironmentVariables()
                    .Build();
            }
            else
            {
                // local dev no Key Vault
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", true)
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .AddEnvironmentVariables()
                   .Build();
            }
        }
    }
}
