using Azure.Identity;
using FunctionIdentityUserAccess;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionIdentityUserAccess;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddScoped<AzureADJwtBearerValidation>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {

        var builtConfig = builder.ConfigurationBuilder.Build();
        var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

        if (!string.IsNullOrEmpty(keyVaultEndpoint))
        {
            // you might need this depending on the dev setup
            var credential = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });

            builder.ConfigurationBuilder
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddAzureKeyVault(new Uri(keyVaultEndpoint), credential)
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
