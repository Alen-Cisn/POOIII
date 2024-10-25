using Txt.Domain.Entities;
using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class CreateNoteLineCommand : ICommand<NoteLine>
{
    public required int NoteId { get; set; }
    public required string Content { get; set; }
    public required int OrderIndex { get; set; }
}