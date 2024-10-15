using MediatR;

namespace Txt.Shared.Commands;

public class CreateNoteCommand : IRequest<int>
{
    public required string Description { get; set; }
}