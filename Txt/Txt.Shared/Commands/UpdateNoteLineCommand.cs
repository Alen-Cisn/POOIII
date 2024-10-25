using Txt.Domain.Entities;
using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class UpdateNoteLineCommand : ICommand<NoteLine>
{
    public required int LineId { get; set; }
    public string? Content { get; set; }
    public int? OrderIndex { get; set; }
}