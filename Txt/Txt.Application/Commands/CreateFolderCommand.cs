using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

public class CreateFolderCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<CreateFolderCommandHandler> logger)
    : ICommandHandler<CreateFolderCommand, FolderDto>
{

    public async Task<OneOf<FolderDto, Error>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string? parentFolderPath = null;
            if (request.ParentId != null)
            {
                Folder folder = await notesModuleRepository
                    .FindFoldersWhere(f => f.Id == request.ParentId)
                    .FirstOrDefaultAsync(cancellationToken)
                    ?? throw new ValidationException("Given parent folder doesn't exist.");
                parentFolderPath = folder.Path;
            }

            Folder note = new()
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Path = parentFolderPath + "/" + request.Name
            };

            Folder entity = notesModuleRepository.CreateFolder(note);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(mapper.Map<FolderDto>(entity));
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}