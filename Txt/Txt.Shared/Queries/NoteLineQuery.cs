using MediatR;
using Txt.Domain.Entities;

namespace Txt.Shared.Queries;

public class NoteLineQuery(int noteId) : IRequest<List<NoteLine>>
{
    public int NoteId { get; set; } = noteId;
}