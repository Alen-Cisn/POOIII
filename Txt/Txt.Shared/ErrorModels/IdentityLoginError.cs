
namespace Txt.Shared.ErrorModels;

public class IdentityLoginError
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Status { get; set; }
    public string Detail { get; set; } = null!;
}
