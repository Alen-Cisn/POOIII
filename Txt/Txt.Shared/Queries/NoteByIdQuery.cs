using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NoteByIdQuery(int id) : IRequest<NoteDto>
{
    public int NoteId { get; set; } = id;
}