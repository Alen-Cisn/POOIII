using Txt.Domain.Entities.Abstract;

namespace Txt.Domain.Entities;

public class Note : Auditable
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int ParentId { get; set; }
    public Folder Parent { get; set; } = null!;
    public required IEnumerable<NoteLine> Lines { get; set; }
}