# Azure Functions Security

## Blogs

Securing Azure Functions using API Keys

https://damienbod.com/2020/08/17/securing-azure-functions-using-api-keys/

# History

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

https://docs.microsoft.com/en-us/azure/virtual-network/tutorial-restrict-network-access-to-resources