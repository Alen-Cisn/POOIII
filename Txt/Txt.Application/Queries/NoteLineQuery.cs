using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteLineQueryHandler(INotesModuleRepository notesModuleRepository)
    : IRequestHandler<NoteLineQuery, List<NoteLine>>
{
    public Task<List<NoteLine>> Handle(NoteLineQuery request, CancellationToken cancellationToken)
    {
        var notes = notesModuleRepository.FindNotesWhere(note => note.Id == request.NoteId);
        if (!notes.Any())
        {
            throw new KeyNotFoundException("Note not found.");
        }

        return notesModuleRepository.FindAllNoteLines(notes.First()).ToListAsync(cancellationToken: cancellationToken);
    }
}