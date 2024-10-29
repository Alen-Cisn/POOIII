using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateFolderCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<UpdateFolderCommand, FolderDto>
{
    public async Task<OneOf<FolderDto, Error>> Handle(UpdateFolderCommand request, CancellationToken cancellationToken)
    {
        Folder parentFolder = await notesModuleRepository
            .FindFoldersWhere(f => f.Id == request.FolderId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ValidationException("Given parent folder doesn't exist.");

        var folder = new Folder
        {
            Id = request.FolderId,
            Name = request.Name,
            ParentId = request.ParentId,
            Path = parentFolder + "/" + request.Name
        };


        notesModuleRepository.UpdateFolder(folder);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<FolderDto>(folder));
    }
}