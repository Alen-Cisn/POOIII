using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
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
    var configurationBuilder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddCommandLine(args);

#if DEBUG
    configurationBuilder.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
#endif

    var configuration = configurationBuilder.Build();

    Logger = logger;

    if (!ValidateConfiguration(configuration))
    {
      throw new FormatException("Configuration was set incorrectly.");
    }

    byte[] iPAddressBytes = GetIpFromDecimalRepresentation(configuration["address"]!);

    var address = new IPAddress(iPAddressBytes);
    var port = int.Parse(configuration["port"]!);

    TcpListener = new TcpListener(address, port);
    ServerCertificate = GetCertificateFromStore(configuration["certificationFingerprint"]!);
  }

  private X509Certificate2 GetCertificateFromStore(string fingerPrint)
  {
    fingerPrint = fingerPrint.Replace(":", "");

    Logger.LogInformation("Looking for certification with fingerPrint \"{}\"", fingerPrint);
    X509Store store = new(StoreLocation.CurrentUser);
    Logger.LogDebug("Certification store location: {}", store.Location.ToString());
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
          X509FindType.FindByThumbprint,
          fingerPrint,
          false
      );

      if (signingCert.Count == 0)
      {
        Logger.LogError("No certification for {} was found.", fingerPrint);
        throw new FileNotFoundException($"No certification for {fingerPrint} was found.", fingerPrint);
      }

      return signingCert[0];
    }
    finally
    {
      store.Close();
    }
  }

  internal void RunServer()
  {
    try
    {
      TcpListener.Start();
      Logger.LogInformation("Server running!");
      
      while (true)
      {
        var client = TcpListener.AcceptTcpClient();

        if (client == null)
        {
          continue;
        }

        _ = HandleRequestAsync(client!);
        Logger.LogInformation("Client request handled!");
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
      string dbPath = "~/.local/share/leveldb";

      // Open the LevelDB database

      // using (var db = new DB(new Options { CreateIfMissing = true }, dbPath))
      {
        // Put a key-value pair into the database
        // db.Put("key1", "value1");
        // Console.WriteLine("Inserted: key1 -> value1");

        // // Retrieve the value for the key
        // string value = db.Get("key1");
        // Console.WriteLine($"Retrieved: key1 -> {value}");

        // // Optionally, delete a key
        // db.Delete("key1");
        // Console.WriteLine("Deleted: key1");
      }
      var sslStream = new SslStream(stream, false);

      try
      {
        const string newLine = "\r\n";
        await sslStream.AuthenticateAsServerAsync(
            ServerCertificate,
            //TODO: Hacer que esto sea requerido. Hay que reparar los certificados SSL.
            clientCertificateRequired: false,
            checkCertificateRevocation: false,
            enabledSslProtocols: SslProtocols.Tls13
        );

        await sslStream.ReadAsync(readBuffer.AsMemory(0, 1024));
        var uriString = Encoding.UTF8.GetString(readBuffer, 0, 1024).Split(newLine)[0];

        Console.WriteLine(uriString);

        var uri = new Uri(uriString);
        // Prepare a response
        string responseBody = "Check!: " + uri.ToString();

        // Send the response back to the client
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseBody);
        await sslStream.WriteAsync(responseBytes);
        await sslStream.FlushAsync(); // Ensure the data is sent
      }
      catch (IOException ex)
      {
        Logger.LogError("IOException: {}", ex.ToString());
      }
      catch (Exception ex)
      {
        Logger.LogError("Exception: {}", ex.ToString());
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
    else if (1024 >= port || port >= 65535)
    {
      Logger.LogError("Port number should be between 1024 and 65535.");
      isValid = false;
    }

    if (configuration["certificationFingerprint"] == null)
    {
      Logger.LogError("Certification fingerprint hasn't been set in settings.");
      isValid = false;
    }
    else if (!ValidateFingerprint(configuration["certificationFingerprint"]!))
    {
      Logger.LogError("Certification fingerprint isn't in the correct format.");
      isValid = false;
    }

    return isValid;
  }

  private static bool ValidateIpAddress(string ipAddressDecimalRepresentation)
      => Ipv4Regex().Matches(ipAddressDecimalRepresentation).Count == 1;

  private static bool ValidateFingerprint(string fingerprint)
      => FingerprintRegex().Matches(fingerprint).Count == 1;

  public void Dispose()
  {
    TcpListener.Stop();
    TcpListener.Dispose();
  }

  [GeneratedRegex("""^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$""")]
  private static partial Regex Ipv4Regex();

  [GeneratedRegex("""^((([0-9A-Fa-f]{2}:){19}[0-9A-Fa-f]{2})|[0-9A-Fa-f]{40})$""")]
  private static partial Regex FingerprintRegex();

}
