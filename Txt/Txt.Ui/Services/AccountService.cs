using System.Net.Http.Json;
using Txt.Shared.Dtos;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class AccountService(HttpClient httpClient) : IAccountService
{
    public Task<AccountInformation?> Get()
    {
        return httpClient.GetFromJsonAsync<AccountInformation>($"account");
    }
}
