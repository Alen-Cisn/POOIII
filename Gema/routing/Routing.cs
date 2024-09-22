

using Gema.Server;
using Gema.Server.Models;

internal static class Routing
{
  public static IServiceCollection AddRoutes(this IServiceCollection services)
  {
    Dictionary<string, Func<Request, Task<IResponse>>> handlers = [];

    handlers.Add("hola", req =>
    {
      return Task.FromResult<IResponse>(new CapsuleResponse()
      {
        StatusCode = 20,
        Capsule = new()
        {
          Id = 0,
          Body = "Capsula estática!"
        }
      });
    });

    services.AddSingleton<IRequestHandler>(new RequestHandler(handlers));
    return services;
  }
}