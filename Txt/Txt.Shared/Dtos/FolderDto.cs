
namespace Txt.Shared.Dtos;

public class FolderDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Path { get; set; } = null!;
    public int? ParentId { get; set; }
    public FolderDto? Parent { get; set; }
    public IEnumerable<FolderDto>? ChildrenFolders { get; set; }
    public IEnumerable<NoteDto>? ChildrenNotes { get; set; }
}