# Azure Functions Security

[![.NET](https://github.com/damienbod/AzureFunctionsSecurity/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/AzureFunctionsSecurity/actions/workflows/dotnet.yml)

## Blogs

[Securing Azure Functions using API Keys](https://damienbod.com/2020/08/17/securing-azure-functions-using-api-keys/)

[Securing Azure Functions using Certificate authentication](https://damienbod.com/2020/09/04/securing-azure-functions-using-certificate-authentication/)

[Securing Azure Functions using an Azure Virtual Network](https://damienbod.com/2020/09/10/securing-azure-functions-using-an-azure-virtual-network/)

[Securing Azure Key Vault inside a VNET and using from an Azure Function](https://damienbod.com/2020/09/16/securing-azure-key-vault-inside-a-vnet-and-using-from-an-azure-function/)

[Securing Azure Functions using Microsoft Entra ID JWT Bearer token authentication for user access tokens](https://damienbod.com/2020/09/24/securing-azure-functions-using-azure-ad-jwt-bearer-token-authentication-for-user-access-tokens/)

## History

- 2024-05-04 Update packages
- 2024-10-31 Update packages
- 2024-10-26 Update packages
- 2024-07-05 Updated user delegated access v4 .NET 8 Azure function, network function updated to v4
- 2024-07-04 Updated FunctionCertificate to v4 project, .NET 8
- 2024-07-03 Updated FunctionApiKeys to v4 project, .NET 8
- 2021-03-07 Update packages and using DefaultAzureCredential for Azure Key vault access,  Microsoft.Identity.Web to 1.7.0
- 2020-10-25 Updated Microsoft.Identity.Web to 1.2.0, Updated Nuget packages
- 2020-09-30 Updated Microsoft.Identity.Web to 1.0.0
- 2020-09-19 Updated Azure Functions configurations to recommended way
- 2020-09-19 Added Azure Function oauth security example user access tokens
- 2020-09-10 Added Azure Function network security example
- 2020-09-01 Added Certificate authentication for Azure Functions

# Testing

## Azure Functions API keys , AuthorizationLevel.Anonymous

### Azure

https://functionssecurity.azurewebsites.net/api/RandomStringAuthLevelAdmin

https://functionssecurity.azurewebsites.net/api/RandomStringAuthLevelAnonymous

https://functionssecurity.azurewebsites.net/api/RandomStringAuthLevelFunc

### Local

http://localhost:7071/api/RandomStringAuthLevelAdmin

http://localhost:7071/api/RandomStringAuthLevelAnonymous

http://localhost:7071/api/RandomStringAuthLevelFunc

## Functions Certificate

### Azure

https://functioncertificate20200829215001.azurewebsites.net/api/randomString

# Links

https://docs.microsoft.com/en-us/azure/azure-functions/security-concepts

https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth

https://damienbod.com/2019/06/13/certificate-authentication-in-asp-net-core-3-0/

https://damienbod.com/2019/09/07/using-certificate-authentication-with-ihttpclientfactory-and-httpclient/

https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/Certificate/src/CertificateAuthenticationHandler.cs
                
https://stackoverflow.com/questions/27307322/verify-server-certificate-against-self-signed-certificate-authority

https://stackoverflow.com/questions/24107374/ssl-certificate-not-in-x509store-when-uploaded-to-azure-website#34719216

https://docs.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth#access-client-certificate


# Links Azure Networking / Application Gateway

https://docs.microsoft.com/en-us/azure/virtual-network/

https://docs.microsoft.com/en-us/azure/virtual-network/tutorial-restrict-network-access-to-resources

https://docs.microsoft.com/en-us/azure/virtual-network/quickstart-create-nat-gateway-portal

http://www.subnet-calculator.com/

https://www.youtube.com/watch?v=8Wh6ZXf8LK8


# OpenID Connect Microsoft Entra ID

https://cmatskas.com/create-an-azure-ad-protected-api-that-calls-into-cosmosdb-with-azure-functions-and-net-core-3-1/

https://anthonychu.ca/post/azure-functions-app-service-openid-connect-auth0/

https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-openid-connect

https://github.com/Azure/azure-functions-vs-build-sdk/issues/397

https://blog.wille-zone.de/post/secure-azure-functions-with-jwt-token/#secure-azure-functions-with-jwt-access-tokens

https://github.com/AzureAD/microsoft-identity-web

https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2

https://jwt.io/
