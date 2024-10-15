using Txt.Domain.Entities.Abstract;

namespace Txt.Domain.Entities;

public class Note : Auditable
{
    public int Id { get; set; }
    public required string Description { get; set; }
}