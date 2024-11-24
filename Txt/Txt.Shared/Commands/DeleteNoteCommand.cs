using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class DeleteNoteCommand : ICommand<string>
{
    public required int NoteId { get; set; }
}