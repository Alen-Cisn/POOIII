using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NoteLinesByNoteIdQuery(int noteId) : IRequest<List<NoteLineDto>>
{
    public int NoteId { get; set; } = noteId;
}