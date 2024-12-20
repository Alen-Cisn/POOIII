
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Helpers;

internal class AuthorizationHandler(
    ILocalStorageService localStorage,
    NavigationManager navigationManager,
    IAuthService authService
) : DelegatingHandler
{
    const int minutesSpan = 2;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string? accessToken = null!;
        if (await IsAccessTokenExpiringSoon(cancellationToken))
        {
            var refreshToken = await localStorage.GetItemAsync<string>("refreshToken", cancellationToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                navigationManager.NavigateTo("/login");
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                };
            }

            accessToken = await authService.RefreshSession(refreshToken, cancellationToken);
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
