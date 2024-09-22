using Cassandra.Geometry;
using Gema.Server;

namespace Gema;

internal sealed class Gema(ServerBase serverBase) : BackgroundService
{
  private readonly ServerBase _serverBase = serverBase ?? throw new ArgumentNullException(nameof(serverBase));

  public static void Main(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);
    var configuration = LoadConfiguration(args);

    builder.Services.AddLogging(loggingBuilder =>
    {
      loggingBuilder.AddConsole();
    });

    builder.Services.AddRoutes();

    var useSqlServerString = configuration["useSqlServer"];

    _ = bool.TryParse(useSqlServerString, out bool useSqlServer);

    if (useSqlServer)
    {
      builder.Services.AddScoped<IRepository, SqlServerRepository>();
    }
    else
    {
      builder.Services.AddScoped(sp =>
        {
          var cluster = Cassandra.Cluster.Builder()
              .AddContactPoint("127.0.0.1")
              .Build();

          return cluster.Connect();
        });

      builder.Services.AddScoped<IRepository, CassandraRepository>();
    }

    builder.Services.AddSingleton(sp =>
    {
      var logger = sp.GetRequiredService<ILogger<ServerBase>>();
      var requestRouter = sp.GetRequiredService<IRequestHandler>();
      var repository = sp.GetRequiredService<IRepository>();
      return new ServerBase(logger, repository, requestRouter, configuration);
    });


    builder.Services.AddHostedService<Gema>();
    var host = builder.Build();
    ValidateConfiguration(configuration, host.Services.GetRequiredService<ILogger<Gema>>());

    host.Run();
  }

  private static IConfiguration LoadConfiguration(string[] args)
  {
    var configurationBuilder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

#if DEBUG
    configurationBuilder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#endif

    configurationBuilder.AddCommandLine(args);
    return configurationBuilder.Build();
  }

  private static void ValidateConfiguration(IConfiguration configuration, ILogger logger)
  {
    var validator = new ConfigurationValidator(logger);
    if (!validator.Validate(configuration))
    {
      throw new FormatException("Configuration was set incorrectly.");
    }
  }

  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      return _serverBase.RunServerAsync();
    }
    catch (Exception ex)
    {
      var logger = _serverBase.Logger;
      logger.LogError(ex, "An error occurred while running the server: {}.", ex.Message);
      throw;
    }
  }
}
