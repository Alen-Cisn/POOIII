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
        string? parentFolderPath = null;
        if (request.ParentFolderId != null)
        {
            Folder folder = await notesModuleRepository
                .FindFoldersWhere(f => f.Id == request.ParentFolderId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given parent folder doesn't exist.");
            parentFolderPath = folder.Path;
        }

        Folder note = new()
        {
            Name = request.Name,
            ParentId = request.ParentFolderId,
            Path = parentFolderPath + "/" + request.Name
        };

        EntityEntry<Folder> entry = notesModuleRepository.CreateFolder(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<FolderDto>(entry.Entity));
    }
}