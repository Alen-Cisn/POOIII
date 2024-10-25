using Txt.Domain.Entities.Abstract;

namespace Txt.Domain.Entities;

public class Folder : Auditable
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? ParentId { get; set; }
    public Folder? Parent { get; set; }
}