
using Txt.Domain.Enums;

namespace Txt.Domain.Entities.Abstract;

public interface ITraceable
{
    public string Path { get; }
    public int? ParentId { get; }
    public Folder? Parent { get; }
    public TraceableEntityType Type { get; }

}