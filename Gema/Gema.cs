using System.ServiceProcess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gema;

public sealed class Gema : ServiceBase
{
    Server? server;

    protected override void OnStart(string[] args)
    {
        try
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });

            server = new(args);
            server.RunServer();
        }
        catch (System.Exception)
        {
            Stop();
            throw;
        }
    }

    protected override void OnStop()
    {
        server?.Dispose();
    }
}
