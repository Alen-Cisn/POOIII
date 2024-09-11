using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Gema.Server;

public partial class ServerBase : IDisposable
{
    private readonly TcpListener TcpListener;
    private readonly X509Certificate2 ServerCertificate;

    private readonly ILogger<ServerBase> Logger;

    public ServerBase(ILogger<ServerBase> logger, string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddCommandLine(args)
            .Build();

        Logger = logger;

        if (!ValidateConfiguration(configuration))
        {
            throw new FormatException("Configuration was set incorrectly.");
        }

        byte[] iPAddressBytes = GetIpFromDecimalRepresentation(configuration["address"]!);

        var address = new IPAddress(iPAddressBytes);
        var port = int.Parse(configuration["port"]!);

        TcpListener = new TcpListener(address, port);
        ServerCertificate = GetCertificateFromStore("localhost");
    }

    private X509Certificate2 GetCertificateFromStore(string certName)
    {
        X509Store store = new(StoreLocation.CurrentUser);
        try
        {
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates;
            X509Certificate2Collection currentCerts = certCollection.Find(
                X509FindType.FindByTimeValid,
                DateTime.Now,
                false
            );

            X509Certificate2Collection signingCert = currentCerts.Find(
                X509FindType.FindBySubjectDistinguishedName,
                certName,
                false
            );

            if (signingCert.Count == 0)
            {
                Logger.LogError("No certification for {} was found.", certName);
                throw new FileNotFoundException($"No certification for {certName} was found.", certName);
            }

            return signingCert[0];
        }
        finally
        {
            store.Close();
        }
    }

    internal async void RunServer()
    {
        try
        {
            TcpListener.Start();
            while (true)
            {
                var client = await TcpListener.AcceptTcpClientAsync();
                //new Thread(() => HandleRequest(client)).Start();
                _ = HandleRequestAsync(client);
            }
        }
        catch (SocketException)
        {
            throw;
        }
    }

    private async Task HandleRequestAsync(TcpClient client)
    {
        var readBuffer = new byte[1024];

        using (var stream = client.GetStream())
        {
            var sslStream = new SslStream(stream, false);

            try
            {
                const string newLine = "\r\n";
                await sslStream.AuthenticateAsServerAsync(
                    ServerCertificate,
                    clientCertificateRequired: true,
                    checkCertificateRevocation: true
                );

                await sslStream.ReadAsync(readBuffer.AsMemory(0, 1024));
                var uriString = Encoding.UTF8.GetString(readBuffer, 0, 1024).Split(newLine)[0];

                Console.WriteLine(uriString);

                var uri = new Uri(uriString);

                //[...]
                // will be covered in the next sections
                //[...]
            }
            catch (IOException ex)
            {
                // handle your exceptions
            }
            finally
            {
                sslStream.Close();
                sslStream.Flush();
            }
        }

        client.Close();
        client.Dispose();
    }

    private static byte[] GetIpFromDecimalRepresentation(string decimalRepresentation)
        => decimalRepresentation.Split('.').Select(byte.Parse).ToArray();
    
    private bool ValidateConfiguration(IConfiguration configuration)
    {
        bool isValid = true;
        if (configuration["address"] == null)
        {
            Logger.LogError("IP address hasn't been set in settings.");
            isValid = false;
        }
        else if (!ValidateIpAddress(configuration["address"]!))
        {
            Logger.LogError("IP address isn't in the correct IPv4 format.");
            isValid = false;
        }
        
        if (configuration["port"] == null)
        {
            Logger.LogError("Port hasn't been set in settings.");
            isValid = false;
        }
        else if (!int.TryParse(configuration["port"], out int port))
        {
            Logger.LogError("Port number isn't a number.");
            isValid = false;
        }
        else if (1024 >= port || port >= 65535 )
        {
            Logger.LogError("Port number should be between 1024 and 65535.");
            isValid = false;
        }

        return isValid;
    }

    private static bool ValidateIpAddress(string ipAddressDecimalRepresentation)
        => Ipv4Regex().Matches(ipAddressDecimalRepresentation).Count == 1;

    public void Dispose()
    {
        TcpListener.Stop();
        TcpListener.Dispose();
    }

    [GeneratedRegex("""^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$""")]
    private static partial Regex Ipv4Regex();

}
