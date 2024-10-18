
namespace Txt.Ui.Models;

public sealed class AccessTokenResponse
{
    public string TokenType { get; } = null!;
    public required string AccessToken { get; init; }
    public required long ExpiresIn { get; init; }
    public required string RefreshToken { get; init; }
}