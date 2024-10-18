

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Txt.Application.Services.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Application.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? Email => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    private static readonly string[] claimsToShow =
    [
        ClaimTypes.Email,
        ClaimTypes.NameIdentifier,
        ClaimTypes.GivenName,
        ClaimTypes.Country,
        ClaimTypes.Name,
    ];

    public IEnumerable<ClaimDto>? Claims => httpContextAccessor.HttpContext?.User?
        .Claims
        .Where(c => claimsToShow.Contains(c.Type))
        .Select(c => new ClaimDto
        {
            Type = c.Type,
            Value = c.Value,
            ValueType = c.ValueType,
            OriginalIssuer = c.OriginalIssuer,
            Issuer = c.Issuer,
        });
}