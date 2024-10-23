
namespace Txt.Ui.Services.Interfaces;

public interface IAuthService
{
    public Task LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task RegisterAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<string?> RefreshSession(string refreshToken, CancellationToken cancellationToken = default);
    public Task LogoutAsync(CancellationToken cancellationToken = default);
}
