using System.Net.Sockets;
using Gema.Server.Models;

namespace Gema.Server;

internal interface IRequestHandler
{
  Task<IResponse> HandleRequestAsync(Request request);
}