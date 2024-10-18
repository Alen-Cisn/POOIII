
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Txt.Ui.Models;

namespace Txt.Ui.Helpers;

internal class AuthorizationHandler(ILocalStorageService localStorage, HttpClient client, IServiceProvider serviceProvider) : DelegatingHandler
{
    const int minutesSpan = 2;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? accessToken = null;

        if (await IsAccessTokenExpiringSoon(cancellationToken))
        {
            var refreshToken = await localStorage.GetItemAsync<string>("refreshToken", cancellationToken);

            var response = await client.PostAsJsonAsync("/refresh-token", new
            {
                RefreshToken = refreshToken
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken);
                if (result != null)
                {
                    await localStorage.SetItemAsync("accessToken", result.AccessToken, cancellationToken);
                    await localStorage.SetItemAsync("refreshToken", result.RefreshToken, cancellationToken);
                    await localStorage.SetItemAsync("expiresOn",
                    DateTime.Now.Add(TimeSpan.FromSeconds(result.ExpiresIn)), cancellationToken);

                    var authStateProvider = (AuthenticationStateProvider?)serviceProvider.GetService(typeof(Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider));
                    authStateProvider?.NotifyUserAuthentication(result.AccessToken);

                    accessToken = result.AccessToken;
                }
            }
        }

        accessToken ??= await localStorage.GetItemAsync<string>("accessToken", cancellationToken);

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<bool> IsAccessTokenExpiringSoon(CancellationToken cancellationToken)
    {
        var expiresOn = await localStorage.GetItemAsync<DateTime>("expiresOn", cancellationToken);

        return DateTime.Now > expiresOn.Add(TimeSpan.FromMinutes(minutesSpan));
    }
}
