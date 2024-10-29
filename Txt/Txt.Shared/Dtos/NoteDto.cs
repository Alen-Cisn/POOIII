namespace Txt.Shared.Dtos;

public class NoteDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Path { get; set; } = null!;
    public required int ParentId { get; set; }
    public FolderDto? Parent { get; set; } = null!;
    public required IEnumerable<NoteLineDto>? Lines { get; set; }
}