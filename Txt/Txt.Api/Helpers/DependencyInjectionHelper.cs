
using Txt.Application.Services;
using Txt.Application.Services.Interfaces;
using Txt.Domain.Repositories.Interfaces;
using Txt.Infrastructure.Repositories;

namespace Txt.Api.Helpers;
public static class DependencyInjectionHelper
{
    public static IServiceCollection AddLocalServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotesRepository, NotesRepository>();
        return services;
    }
}