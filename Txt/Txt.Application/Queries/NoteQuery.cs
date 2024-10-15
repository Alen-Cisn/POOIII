using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteQueryHandler(INotesRepository notesRepository)
    : IRequestHandler<NoteQuery, List<Note>>
{
    public Task<List<Note>> Handle(NoteQuery request, CancellationToken cancellationToken)
    {
        return notesRepository.FindAll().ToListAsync(cancellationToken: cancellationToken);
    }
}