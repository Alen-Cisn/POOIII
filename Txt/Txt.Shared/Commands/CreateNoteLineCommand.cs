using Txt.Shared.Commands.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Shared.Commands;

public class CreateNoteLineCommand : ICommand<NoteLineDto>
{
    public required int NoteId { get; set; }
    public required string Content { get; set; }
    public required int OrderIndex { get; set; }
}