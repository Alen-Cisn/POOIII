using System.Net.Http.Json;
using Txt.Shared.Dtos;
using Txt.Ui.Services.HttpClients.Interfaces;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class AccountService(ITxtApiClientService clientService) : IAccountService
{
    private HttpClient HttpClient { get; init; } = clientService.HttpClient;

    public Task<AccountInformation?> Get()
    {
        return HttpClient.GetFromJsonAsync<AccountInformation>($"account");
    }

    public Task<IEnumerable<ClaimDto>?> GetClaims()
    {
        return HttpClient.GetFromJsonAsync<IEnumerable<ClaimDto>>($"claims");
    }
}
