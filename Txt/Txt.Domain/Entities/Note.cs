using Txt.Domain.Entities.Abstract;
using Txt.Domain.Enums;

namespace Txt.Domain.Entities;

public class Note : Auditable, ITraceable
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Path { get; set; } = null!;
    public Folder Parent { get; set; } = null!;
    int? ITraceable.ParentId => ParentId;
    public required int ParentId { get; set; }
    public required IEnumerable<NoteLine> Lines { get; set; }
    public TraceableEntityType Type { get => TraceableEntityType.Note; }
}