using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Application.Services.Interfaces;
using Txt.Shared.Dtos;
namespace Txt.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AccountController(ICurrentUserService currentUserService) : ControllerBase
{

    [HttpGet]
    public AccountInformation Get()
        => new()
        {
            UserId = currentUserService.UserId!,
            Email = currentUserService.Email!,
        };

    [HttpGet("claims")]
    public IEnumerable<ClaimDto>? GetClaims()
        => currentUserService.Claims;

}
