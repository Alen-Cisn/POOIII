using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Exceptions;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteByPathQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<NoteByPathQuery, NoteDto>
{
    public async Task<NoteDto> Handle(NoteByPathQuery request, CancellationToken cancellationToken)
    {
        Note notes = await notesModuleRepository.FindNotesWhere(note =>
            note.Path == request.Path
        )
        .Include(note => note.Lines)
        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Note not found.");

        return mapper.Map<NoteDto>(notes);
    }
}