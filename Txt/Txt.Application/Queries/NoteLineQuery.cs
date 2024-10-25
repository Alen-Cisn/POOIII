using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteLineQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<NoteLineQuery, List<NoteLineDto>>
{
    public async Task<List<NoteLineDto>> Handle(NoteLineQuery request, CancellationToken cancellationToken)
    {
        var notes = notesModuleRepository.FindNotesWhere(note => note.Id == request.NoteId);
        if (!notes.Any())
        {
            throw new KeyNotFoundException("Note not found.");
        }

        List<NoteLine> noteLines = await notesModuleRepository.FindAllNoteLines(notes.First()).ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<NoteLineDto>>(noteLines);
    }
}