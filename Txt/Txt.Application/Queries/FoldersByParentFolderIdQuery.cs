using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class FoldersByParentFolderIdQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<FoldersByParentFolderIdQuery, List<FolderDto>>
{
    public async Task<List<FolderDto>> Handle(FoldersByParentFolderIdQuery request, CancellationToken cancellationToken)
    {
        List<Folder> Folders = await notesModuleRepository.FindFoldersWhere(Folder =>
            Folder.ParentId == request.FolderId
        ).ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<FolderDto>>(Folders);
    }
}