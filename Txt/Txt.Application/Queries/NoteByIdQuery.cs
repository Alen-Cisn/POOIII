using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteByIdQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<NoteByIdQuery, NoteDto>
{
    public async Task<NoteDto> Handle(NoteByIdQuery request, CancellationToken cancellationToken)
    {
        Note notes = await notesModuleRepository.FindNotesWhere(note =>
            note.Id == request.NoteId
        ).FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Note not found.");

        return mapper.Map<NoteDto>(notes);
    }
}