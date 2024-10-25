using Txt.Shared.Commands.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Shared.Commands;

public class UpdateNoteLineCommand : ICommand<NoteLineDto>
{
    public required int NoteId { get; set; }
    public required int LineId { get; set; }
    public string? Content { get; set; }
    public int? OrderIndex { get; set; }
}