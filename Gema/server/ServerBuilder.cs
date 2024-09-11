using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gema.Server;

class ServerBuilder(IServiceCollection services)
{

    private readonly IServiceCollection _services = services;

    public ServerBuilder AddDependency<T>() where T : class, ILogger<ServerBase>
    {
        _services.AddTransient<ILogger<ServerBase>, T>();
        return this;
    }

    public ServerBuilder AddArgs(string[] args)
    {
        _services.AddSingleton(args);
        return this;
    }

    public ServerBase Build()
    {
        var serviceProvider = _services.BuildServiceProvider();
        return new ServerBase(serviceProvider.GetRequiredService<ILogger<ServerBase>>(),
            serviceProvider.GetRequiredService<string[]>());
    }
}
