namespace Gema.Server.Models;

internal interface IResponse {
  public byte StatusCode { get; }
  public Capsule? Capsule { get; }

}