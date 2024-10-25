using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NoteLineQuery(int noteId) : IRequest<List<NoteLineDto>>
{
    public int NoteId { get; set; } = noteId;
}