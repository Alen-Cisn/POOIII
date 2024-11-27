
using MediatR;
using Txt.Application.Helpers;
using Txt.Application.PipelineBehaviors;
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
        services.AddAutoMapper(typeof(AutoMapperProfile));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotesModuleRepository, NotesModuleRepository>();
        return services;
    }
}