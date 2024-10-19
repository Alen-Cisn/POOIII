using Txt.Ui.Services;
using Txt.Ui.Services.HttpClients;
using Txt.Ui.Services.HttpClients.Interfaces;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Helpers;
public static class DependencyInjectionHelper
{
    public static IServiceCollection AddLocalServices(this IServiceCollection services)
    {
        services.AddScoped<IPublicClientService, PublicClientService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}