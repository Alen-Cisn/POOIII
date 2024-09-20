using Gema.Server;

namespace Gema;

public sealed class Gema(ServerBase serverBase) : BackgroundService
{
  private readonly ServerBase _serverBase = serverBase ?? throw new ArgumentNullException(nameof(serverBase));

  public static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddLogging(loggingBuilder =>
    {
      loggingBuilder.AddConsole();
    });

    builder.Services.AddSingleton(sp =>
    {
      var logger = sp.GetRequiredService<ILogger<ServerBase>>();
      return new ServerBase(logger, args);
    });

    builder.Services.AddHostedService<Gema>();

    var host = builder.Build();
    host.Run();
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      return _serverBase.RunServerAsync();
    }
    catch (Exception)
    {
      // Log the exception or handle it as needed
      throw;
    }
  }
}
