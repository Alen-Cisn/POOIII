using System.Net.Http.Json;
using Txt.Shared.Dtos;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class AuthService(HttpClient httpClient) : IAuthService
{
    public Task<AccountInformation?> Login()
    {
        return httpClient.GetFromJsonAsync<AccountInformation>($"account");
    }
}
