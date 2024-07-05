using Azure.Identity;
using FunctionIdentityUserAccess;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureHostConfiguration(builder =>
    {
        var builtConfig = builder.Build();
        var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

        if (!string.IsNullOrEmpty(keyVaultEndpoint))
        {
            // you might need this depending on the dev setup
            var credential = new DefaultAzureCredential(
                new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true });

            // using Key Vault, either local dev or deployed
            builder.SetBasePath(Environment.CurrentDirectory)
                .AddAzureKeyVault(new Uri(keyVaultEndpoint), new DefaultAzureCredential())
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
        else
        {
            // local dev no Key Vault
            builder.SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddEnvironmentVariables()
            .Build();
        }
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<EntraIDJwtBearerValidation>();
    })
    .Build();

host.Run();
