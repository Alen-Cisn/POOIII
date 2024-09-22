namespace Gema.Server.Models;

internal class NotFoundResponse: IResponse
{
  public byte StatusCode => 40;

  public Capsule? Capsule => null;

}