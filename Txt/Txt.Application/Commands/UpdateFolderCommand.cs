using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Txt.Application.Commands.Interfaces;
using Txt.Application.PipelineBehaviors;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateFolderCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<UpdateFolderCommandHandler> logger)
    : ICommandHandler<UpdateFolderCommand, FolderDto>
{
    public async Task<OneOf<FolderDto, Error>> Handle(UpdateFolderCommand request, CancellationToken cancellationToken)
    {
        try
        {

            Folder folder = await notesModuleRepository
                .FindFoldersWhere(f => f.Id == request.FolderId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("The given folder doesn't exist.");

            folder.Name = request.Name;

            if (folder.ParentId != request.ParentId && request.ParentId != null)
            {
                Folder parentFolder = await notesModuleRepository
                    .FindFoldersWhere(f => f.Id == request.ParentId)
                    .FirstOrDefaultAsync(cancellationToken)
                    ?? throw new NotFoundException("The given parent folder doesn't exist.");

                folder.ParentId = parentFolder.Id;
                folder.Path = parentFolder.Path + "/" + request.Name;
            }
            else
            {
                int lastIndex = folder.Path.LastIndexOf('/');
                if (lastIndex != -1)
                {
                    folder.Path = folder.Path[..(lastIndex + 1)] + request.Name;
                }
            }

            if (await notesModuleRepository
                .FindFoldersWhere(f => f.Path == folder.Path && f.Id != folder.Id)
                .AnyAsync(cancellationToken))
            {
                throw new ValidationException("Given folder already exists.");
            }

            if (await notesModuleRepository
                .FindNotesWhere(n => n.Path == folder.Path)
                .AnyAsync(cancellationToken))
            {
                throw new ValidationException("Given name already exists as a note.");
            }

            notesModuleRepository.UpdateFolder(folder);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(mapper.Map<FolderDto>(folder));
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}