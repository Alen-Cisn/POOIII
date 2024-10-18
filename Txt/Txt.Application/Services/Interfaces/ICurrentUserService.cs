

using Txt.Shared.Dtos;

namespace Txt.Application.Services.Interfaces;

public interface ICurrentUserService
{
    public string? UserId { get; }
    public string? Email { get; }
    public IEnumerable<ClaimDto>? Claims { get; }
}