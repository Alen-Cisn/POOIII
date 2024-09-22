namespace Gema.Server.Models;

internal class CapsuleResponse: IResponse
{
  public byte StatusCode { get; set; }

  public Capsule? Capsule { get; set; }

}