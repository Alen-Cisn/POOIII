using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class CreateFolderCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<CreateFolderCommand, FolderDto>
{

    public async Task<OneOf<FolderDto, Error>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        Folder folder = await notesModuleRepository
            .FindFoldersWhere(f => f.Id == request.FolderId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ValidationException("Given parent folder doesn't exist.");

        Folder note = new()
        {
            Name = request.Name,
            ParentId = request.FolderId,
            Path = folder.Path + "/" + request.Name
        };

        EntityEntry<Folder> entry = notesModuleRepository.CreateFolder(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<FolderDto>(entry.Entity));
    }
}