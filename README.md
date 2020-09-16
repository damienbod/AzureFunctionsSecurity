# Azure Functions Security

## Blogs

Securing Azure Functions using API Keys

https://damienbod.com/2020/08/17/securing-azure-functions-using-api-keys/

Securing Azure Functions using certificate authentication

https://damienbod.com/2020/09/04/securing-azure-functions-using-certificate-authentication/

Securing Azure Functions using an Azure Virtual Network

https://damienbod.com/2020/09/10/securing-azure-functions-using-an-azure-virtual-network/

Securing Azure Key Vault inside a VNET and using from an Azure Function

https://damienbod.com/2020/09/16/securing-azure-key-vault-inside-a-vnet-and-using-from-an-azure-function/

# History

2020-09-10 Added Azure Function network security example

2020-09-01 Added Certificate authentication for Azure Functions

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
