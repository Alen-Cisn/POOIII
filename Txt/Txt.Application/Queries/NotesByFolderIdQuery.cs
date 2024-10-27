using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class NotesByFolderIdQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<NotesByFolderIdQuery, List<NoteDto>>
{
    public async Task<List<NoteDto>> Handle(NotesByFolderIdQuery request, CancellationToken cancellationToken)
    {
        List<Note> notes = await notesModuleRepository.FindNotesWhere(note =>
            note.ParentId == request.FolderId
        ).ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<NoteDto>>(notes);
    }
}