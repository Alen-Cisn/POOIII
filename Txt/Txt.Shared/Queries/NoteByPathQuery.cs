using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NoteByPathQuery : IRequest<NoteDto>
{
    public string Path { get; set; }
}