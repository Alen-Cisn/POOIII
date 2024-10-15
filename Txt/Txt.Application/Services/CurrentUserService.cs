

using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Txt.Application.Services.Interfaces;

namespace Txt.Application.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}