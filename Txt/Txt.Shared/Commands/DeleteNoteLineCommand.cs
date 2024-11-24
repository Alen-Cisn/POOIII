using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class DeleteNoteLineCommand : ICommand<string>
{
    public required int NoteId { get; set; }
    public required int LineId { get; set; }
}