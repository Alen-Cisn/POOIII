using Txt.Domain.Entities.Abstract;
using Txt.Domain.Enums;

namespace Txt.Domain.Entities;

public class Folder : Auditable, ITraceable
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Path { get; set; } = null!;
    public int? ParentId { get; set; }
    public Folder? Parent { get; set; }
    public TraceableEntityType Type { get => TraceableEntityType.Folder; }
}