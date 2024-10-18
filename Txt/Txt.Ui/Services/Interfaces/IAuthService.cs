
using Txt.Shared.Dtos;

namespace Txt.Ui.Services.Interfaces;

public interface IAuthService
{
    public Task<AccountInformation?> Login();
}
