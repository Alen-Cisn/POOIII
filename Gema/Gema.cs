using System.ServiceProcess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gema.Server;

namespace Gema;

public sealed class Gema : ServiceBase
{
    public static void Main(string[] args)
    {
        var isWindowsService = !Environment.UserInteractive;
        var gema = new Gema(args);

        if (isWindowsService && OperatingSystem.IsWindows())
        {
            Run(gema);
        }
        else
        {
            gema.OnStart(args);
            gema.OnStop();
        }
    }

    private Gema(string[] args)
    {

        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddConsole();
        });
        
        ServerBuilder serverBuilder = new(services);
        serverBuilder.AddArgs(args);

        ServerBase = serverBuilder.Build();
    }

    readonly ServerBase ServerBase;

    protected override void OnStart(string[] args)
    {
        try
        {
            ServerBase.RunServer();
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            if (OperatingSystem.IsWindows())
            {
                Stop();
            }
        }
    }

    protected override void OnStop()
    {
        ServerBase.Dispose();
    }
}
