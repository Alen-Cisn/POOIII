using Txt.Ui.Services;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Helpers;
public static class DependencyInjectionHelper
{
    public static IServiceCollection AddLocalServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}