

namespace Txt.Shared.Dtos;

public class NoteLineDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public required int NoteId { get; set; }
    public NoteDto? Note { get; set; }
    public required int OrderIndex { get; set; }
}