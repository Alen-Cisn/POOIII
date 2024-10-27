
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NoteLinesByNoteIdQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<NoteLinesByNoteIdQuery, List<NoteLineDto>>
{
    public async Task<List<NoteLineDto>> Handle(NoteLinesByNoteIdQuery _, CancellationToken cancellationToken)
    {
        var lines = await notesModuleRepository.FindAllNoteLines(_.NoteId).ToListAsync(cancellationToken);

        return mapper.Map<List<NoteLineDto>>(lines);
    }
}