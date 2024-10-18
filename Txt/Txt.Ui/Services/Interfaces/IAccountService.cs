
using Txt.Shared.Dtos;

namespace Txt.Ui.Services.Interfaces;

public interface IAccountService
{
    public Task<AccountInformation?> Get();
}
