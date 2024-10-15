
using Microsoft.Extensions.DependencyInjection;

namespace Txt.Application.Helpers;
public static class ServicesHelper
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        return services;
    }
}