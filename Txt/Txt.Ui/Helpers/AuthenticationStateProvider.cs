using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Txt.Shared.Dtos;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Helpers;

internal class AuthenticationStateProvider(IAccountService accountService)
    : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
{

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var claimDtos = await accountService.GetClaims();

        var identity = claimDtos == null
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ToClaimEnumerable(claimDtos), "claims");

        var authenticatedUser = new ClaimsPrincipal(identity);
        return new AuthenticationState(authenticatedUser);
    }

    public Task NotifyUserAuthentication()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }

    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private static IEnumerable<Claim> ToClaimEnumerable(IEnumerable<ClaimDto> claimDtos)
    {
        return claimDtos.Select(dto => new Claim(dto.Type, dto.Value, dto.ValueType, dto.Issuer, dto.OriginalIssuer));
    }
}
