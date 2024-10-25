
namespace Txt.Shared.ErrorModels;

public class Error
{
    public int ErrorCode { get; set; }
    public string Details { get; set; } = null!;
}