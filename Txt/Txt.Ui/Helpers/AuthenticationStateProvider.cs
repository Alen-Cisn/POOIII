using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using Txt.Shared.Dtos;

namespace Txt.Ui.Helpers;

internal class AuthenticationStateProvider(ILocalStorageService localStorage, HttpClient client) : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>("accessToken");

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(await RetreiveClaims(token), "claims");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(await RetreiveClaims(token), "claims"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<IEnumerable<Claim>?> RetreiveClaims(string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        var enumerableClaimDto = await client
            .GetFromJsonAsync<IEnumerable<ClaimDto>>("http://localhost:5000/account/claims");
        return enumerableClaimDto?.Select(dto => new Claim(dto.Type, dto.Value, dto.ValueType, dto.Issuer, dto.OriginalIssuer));
    }
}
