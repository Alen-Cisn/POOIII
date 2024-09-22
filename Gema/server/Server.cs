using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Gema.Server;

internal partial class ServerBase : IDisposable
{
  internal readonly ILogger<ServerBase> Logger;
  private readonly IRequestHandler _requestHandler;
  private readonly IRepository _repository;

  private readonly TcpListener _tcpListener;
  private readonly X509Certificate2 _serverCertificate;


  public ServerBase(ILogger<ServerBase> logger, IRepository repository, IRequestHandler requestHandler, IConfiguration configuration)
  {
    Logger = logger;
    _requestHandler = requestHandler;
    _repository = repository;

    var address = GetIPAddress(configuration["address"]!);
    var port = int.Parse(configuration["port"]!);

    _tcpListener = new TcpListener(address, port);
    _serverCertificate = GetCertificateFromStore(configuration["certificationFingerprint"]!);
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

  internal async Task RunServerAsync()
  {
    try
    {
      _tcpListener.Start();
      Logger.LogInformation("Server running!");

      while (true)
      {
        var client = await _tcpListener.AcceptTcpClientAsync();
        _ = HandleRequestAsync(client);
      }
    }
    catch (SocketException ex)
    {
      Logger.LogError("SocketException: {Message}", ex.Message);
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
            _serverCertificate,
            //TODO: Hacer que esto sea requerido. Hay que reparar los certificados SSL.
            clientCertificateRequired: false,
            checkCertificateRevocation: false,
            enabledSslProtocols: SslProtocols.Tls13
        );

        await sslStream.ReadAsync(readBuffer.AsMemory(0, 1024));
        var uriString = Encoding.UTF8.GetString(readBuffer, 0, 1024).Split(newLine)[0];


        var uri = new Uri(uriString);
        var response = _requestHandler.HandleRequestAsync(new Request() { Uri = uri });

        Logger.LogInformation("Got uri: {}", uri.ToString());
        // Prepare a response
        string responseBody = "Check!: " + uri.ToString();

        // Send the response back to the client
        byte[] responseBytes = Encoding.UTF8.GetBytes(responseBody);
        await sslStream.WriteAsync(responseBytes);
        await sslStream.FlushAsync(); // Ensure the data is sent
      }
      catch (FormatException ex)
      {
        //... ssl Write bad request error
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

  public void Dispose()
  {
    _tcpListener.Stop();
    _tcpListener.Dispose();
  }

  private static IPAddress GetIPAddress(string decimalRepresentation)
  {
    var bytes = decimalRepresentation.Split('.').Select(byte.Parse).ToArray();
    return new IPAddress(bytes);
  }
}