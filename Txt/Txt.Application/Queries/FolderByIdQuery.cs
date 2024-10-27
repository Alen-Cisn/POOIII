using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class FolderByIdQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<FolderByIdQuery, FolderDto>
{
    public async Task<FolderDto> Handle(FolderByIdQuery request, CancellationToken cancellationToken)
    {
        Folder Folders = await notesModuleRepository.FindFoldersWhere(Folder =>
            Folder.Id == request.FolderId
        ).FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new KeyNotFoundException("Folder not found.");

        return mapper.Map<FolderDto>(Folders);
    }
}