using MediatR;
using Txt.Domain.Entities;

namespace Txt.Shared.Commands;

public class CreateNoteCommand : IRequest<Note>
{
    public required string Description { get; set; }
}