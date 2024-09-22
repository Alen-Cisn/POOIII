using Gema.Server.Models;

namespace Gema.Server;

internal class RequestHandler(Dictionary<string, Func<Request, Task<IResponse>>> handlers): IRequestHandler
{
  private readonly Dictionary<string, Func<Request, Task<IResponse>>> _handlers = handlers;

  // public void RegisterHandler(string path, Func<Request, IResponse> handler)
  // {
  //   _handlers[path] = handler;
  // }

  public async Task<IResponse> HandleRequestAsync(Request request)
  {
    foreach (var kvp in _handlers)
    {
      var route = kvp.Key;
      var handler = kvp.Value;

      if (IsMatchingRoute(request.Uri.LocalPath, route))
      {
        return await handler(request);
      }
    }

    return new NotFoundResponse();
  }

  private static bool IsMatchingRoute(string requestPath, string route)
  {
    var requestSegments = requestPath.Split("?")[0].Trim('/').Split('/');

    var routeSegments = route.Trim('/').Split('/');

    if (requestSegments.Length != routeSegments.Length)
    {
      return false;
    }

    for (int i = 0; i < routeSegments.Length; i++)
    {
      if (routeSegments[i] == "*")
      {
        continue;
      }

      if (routeSegments[i] != requestSegments[i])
      {
        return false;
      }
    }

    return true;
  }
}
