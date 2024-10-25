

namespace Txt.Shared.Dtos;

public sealed class ClaimDto
{
    public string Type { get; set; } = null!;
    public string OriginalIssuer { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string ValueType { get; set; } = null!;
    public string Value { get; set; } = null!;
}