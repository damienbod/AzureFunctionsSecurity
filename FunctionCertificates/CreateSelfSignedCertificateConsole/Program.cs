using CertificateManager;
using CertificateManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CreateSelfSignedCertsConsole;

class Program
{
    static void Main(string[] args)
    {
        var dnsName = "FunctionCertificate";
        var serviceProvider = new ServiceCollection()
            .AddCertificateManager()
            .BuildServiceProvider();

        string password = "1234";
        var importExportCertificate = serviceProvider.GetService<ImportExportCertificate>();
        var createClientServerAuthCerts = serviceProvider.GetService<CreateCertificatesClientServerAuth>();

        // Client self signed certificate
        var client = createClientServerAuthCerts.NewClientSelfSignedCertificate(
            new DistinguishedName
            {
                CommonName = "functionsCertAuth",
                Country = "CH",
                Organisation = "damienbod"
            },
            new ValidityPeriod { ValidFrom = DateTime.UtcNow, ValidTo = DateTime.UtcNow.AddYears(10) },
            dnsName);

        client.FriendlyName = "azure client certificate";

        var clientCertInPfxBtyes =
            importExportCertificate.ExportSelfSignedCertificatePfx(password, client);
        File.WriteAllBytes("functionsCertAuth.pfx", clientCertInPfxBtyes);

        var clientCertInPEMBtyes = importExportCertificate.PemExportPfxFullCertificate(client);
        File.WriteAllText("functionsCertAuth.pem", clientCertInPEMBtyes);

        Console.WriteLine("Certificates exported to pfx and cer files");
    }
}
