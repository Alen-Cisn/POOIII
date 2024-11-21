
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Txt.Application.Commands;
using Txt.Application.PipelineBehaviors;
using Txt.Domain.Entities;
using Txt.Shared.ErrorModels;

namespace Txt.Application.Helpers;
public static class ServicesHelper
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.RegisterServicesFromAssemblies(typeof(Error).Assembly);
            cfg.RegisterServicesFromAssemblies(typeof(Note).Assembly);
            cfg.RegisterServicesFromAssemblyContaining<UpdateFolderCommandHandler>();
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandler<,>), ServiceLifetime.Transient);
        });

        return services;
    }
}