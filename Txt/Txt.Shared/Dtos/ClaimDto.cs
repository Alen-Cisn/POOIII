

namespace Txt.Shared.Dtos;

public sealed class ClaimDto
{
    public string Type { get; set; }
    public string OriginalIssuer { get; set; }
    public string Issuer { get; set; }
    public string ValueType { get; set; }
    public string Value { get; set; }
}