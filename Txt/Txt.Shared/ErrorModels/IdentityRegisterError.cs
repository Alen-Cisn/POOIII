
namespace Txt.Shared.ErrorModels;

public class IdentityRegisterError
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Status { get; set; }
    public string Detail { get; set; } = null!;
    public string Instance { get; set; } = null!;
    public Dictionary<string, List<string>> Errors { get; set; } = null!;
}
