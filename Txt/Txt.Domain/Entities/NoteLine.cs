using Txt.Domain.Entities.Abstract;

namespace Txt.Domain.Entities;

public class NoteLine : Auditable
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public int NoteId { get; set; }
    public Note Note { get; set; } = null!;
    public int OrderIndex { get; set; }
}