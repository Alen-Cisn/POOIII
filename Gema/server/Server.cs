using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Gema;

public class Server : IDisposable
{
    private TcpListener tcpListener;
    private X509Certificate2 serverCertificate;

    public Server(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddCommandLine(args)
            .Build();

        if (!ValidateConfiguration(configuration))
        {
            throw new FormatException("Configuration was incorrectly set.");
        }

        byte[] iPAddressBytes = GetIpFromDecimalRepresentation(configuration["address"]!);

        var address = new IPAddress(iPAddressBytes);
        var port = int.Parse(configuration["port"]!);

        tcpListener = new TcpListener(address, port);
        serverCertificate = GetCertificateFromStore("CERT");
    }

    private static X509Certificate2 GetCertificateFromStore(string certName)
    {
        X509Store store = new X509Store(StoreLocation.CurrentUser);
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
                return null!;

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
            tcpListener.Start();
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
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
        Console.WriteLine("Connected!");
        using (var stream = client.GetStream())
        {
            var sslStream = new SslStream(stream, false);

            try
            {
                const string newLine = "\r\n";
                await sslStream.AuthenticateAsServerAsync(
                    serverCertificate,
                    clientCertificateRequired: true,
                    checkCertificateRevocation: true
                );

                await sslStream.ReadAsync(readBuffer, 0, 1024);
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
    {
        string[] numbers = decimalRepresentation.Split('.');

        byte[] ret = new byte[4];

        for (int i = 0; i < 4; i++)
        {
            ret[i] = byte.Parse(numbers[i]);
        }

        return ret;
    }

    private static bool ValidateConfiguration(IConfiguration configuration)
    {
        if (!ValidateIpAddress(configuration["address"]!))
        {
            return false;
        }

        if (!int.TryParse(configuration["port"], out _))
        {
            return false;
        }

        return true;
    }

    private static bool ValidateIpAddress(string ipAddressDecimalRepresentation)
    {
        return Regex
                .Matches(
                    ipAddressDecimalRepresentation,
                    """^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"""
                )
                .Count() == 1;
    }

    public void Dispose()
    {
        tcpListener.Stop();
        tcpListener.Dispose();
    }
}
