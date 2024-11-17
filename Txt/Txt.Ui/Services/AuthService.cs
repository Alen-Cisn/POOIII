using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Txt.Shared.ErrorModels;
using Txt.Ui.Helpers;
using Txt.Ui.Models;
using Txt.Ui.Services.HttpClients.Interfaces;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class AuthService(
    IPublicClientService publicClientService,
    ILocalStorageService localStorage,
    NavigationManager navigationManager,
    IServiceProvider serviceProvider,
    ISnackbar snackbar,
    ILogger<AuthService> logger
    ) : IAuthService
{
    private HttpClient HttpClient { get; init; } = publicClientService.HttpClient;

    public async Task LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync("/authorization/login", new
            {
                email,
                password,
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken);
                if (result == null)
                {
                    return;
                }

                _ = SaveAndNotifySession(result, cancellationToken);

                var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
                var queryStrings = System.Web.HttpUtility.ParseQueryString(uri.Query);
                var returnUrl = queryStrings.Get("returnUrl");

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var returnUri = new Uri(returnUrl);
                    navigationManager.NavigateTo(returnUri.AbsolutePath);
                }
                else
                {
                    navigationManager.NavigateTo("/");
                }
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<IdentityLoginError>(cancellationToken: cancellationToken);
                if (errorResponse != null)
                {
                    snackbar.Add("Incorrect password or email.", Severity.Error);
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching account information: {Message}", httpEx.Message);
            snackbar.Add(httpEx.Message, Severity.Error);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching account information: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching account information: {Message}", ex.Message);
        }
    }
    public async Task RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync("/authorization/register", new
            {
                email,
                password,
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await LoginAsync(email, password, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var errorResponse = JsonSerializer.Deserialize<IdentityRegisterError>(content);
                if (errorResponse != null)
                {

                    HandleRegisterErrorResponse(errorResponse);
                }
                else
                {
                    snackbar.Add("An unexpected error ocurred.", Severity.Error);
                }
            }
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching account information: {Message}", httpEx.Message);
            snackbar.Add(httpEx.Message, Severity.Error);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching account information: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching account information: {Message}", ex.Message);
        }
    }

    private void HandleRegisterErrorResponse(IdentityRegisterError errorResponse)
    {
        foreach (var message in errorResponse.Errors.SelectMany(e => e.Value))
        {
            snackbar.Add(message, Severity.Error);
        }
    }

    public async Task<string?> RefreshSession(string refreshToken, CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.PostAsJsonAsync("/authorization/refresh-token", new
        {
            RefreshToken = refreshToken
        }, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/login");
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<AccessTokenResponse>(cancellationToken);
        if (result == null)
        {
            return null;
        }

        _ = SaveAndNotifySession(result, cancellationToken);

        return result.AccessToken;
    }

    public Task LogoutAsync(CancellationToken cancellationToken = default)
        => localStorage.RemoveItemsAsync(["accessToken", "refreshToken", "expiresOn"], cancellationToken).AsTask();

    public async Task SaveAndNotifySession(AccessTokenResponse accessTokenResponse, CancellationToken cancellationToken = default)
    {
        await localStorage.SetItemAsync("accessToken", accessTokenResponse.AccessToken, cancellationToken);
        await localStorage.SetItemAsync("refreshToken", accessTokenResponse.RefreshToken, cancellationToken);
        await localStorage.SetItemAsync("expiresOn",
        DateTime.Now.Add(TimeSpan.FromSeconds(accessTokenResponse.ExpiresIn)), cancellationToken);

        var authStateProvider = (AuthenticationStateProvider?)serviceProvider.GetService(typeof(Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider));
        authStateProvider?.NotifyUserAuthentication();
    }
}
