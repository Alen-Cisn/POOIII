using System.ComponentModel.DataAnnotations;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Shared.Commands;

public class UpdateNoteCommand : ICommand<NoteDto>
{
    public required int NoteId { get; set; }
    [Length(1, 180, ErrorMessage = "Name can't be empty.")]
    public required string Name { get; set; }
    public required int ParentId { get; set; }
}