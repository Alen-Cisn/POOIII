namespace Txt.Ui.Shared;

public class FolderOrNote
{
    internal enum TypeEnum
    {
        Folder,
        Note
    }

    internal TypeEnum Type { get; set; }
    internal int Id { get; set; }
    internal string Name { get; set; } = null!;
    internal int? ParentId { get; set; }
}