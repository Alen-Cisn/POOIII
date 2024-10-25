using System.ComponentModel.DataAnnotations;
using Txt.Domain.Entities;
using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class CreateNoteCommand : ICommand<Note>
{
    [Length(1, 180, ErrorMessage = "Name can't be empty.")]
    public required string Name { get; set; }
    public required int FolderId { get; set; }
}