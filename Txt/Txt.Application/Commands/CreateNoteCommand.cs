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

public class CreateNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<CreateNoteCommandHandler> logger)
    : ICommandHandler<CreateNoteCommand, NoteDto>
{

    public async Task<OneOf<NoteDto, Error>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Folder folder = await notesModuleRepository
                .FindFoldersWhere(f => f.Id == request.ParentId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given parent folder doesn't exist.");

            Note note = new()
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Lines = [],
                Path = folder.Path + "/" + request.Name
            };

            Note entity = notesModuleRepository.CreateNote(note);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(mapper.Map<NoteDto>(entity));
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}