using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteQueryHandler(INotesModuleRepository notesModuleRepository)
    : IRequestHandler<NoteQuery, List<Note>>
{
    public Task<List<Note>> Handle(NoteQuery request, CancellationToken cancellationToken)
    {
        return notesModuleRepository.FindNotesWhere(note =>
            (request.Id == null || note.Id == request.Id)
            && (request.FolderId == null || note.ParentId == request.FolderId)
        ).ToListAsync(cancellationToken: cancellationToken);
    }
}