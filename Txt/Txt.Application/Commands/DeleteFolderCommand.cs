using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Txt.Application.Commands.Interfaces;
using Txt.Application.PipelineBehaviors;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class DeleteFolderCommandHandler(INotesModuleRepository notesModuleRepository, ILogger<DeleteFolderCommandHandler> logger)
    : ICommandHandler<DeleteFolderCommand, string>
{

    public async Task<OneOf<string, Error>> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Folder folder = await notesModuleRepository
                .FindFoldersWhere(f => f.Id == request.FolderId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given folder doesn't exist.");

            string folderName = folder.Name;
            notesModuleRepository.DeleteFolder(folder);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(folderName);
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}