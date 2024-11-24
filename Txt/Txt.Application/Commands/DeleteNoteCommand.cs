using AutoMapper;
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

public class DeleteNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<DeleteNoteCommandHandler> logger)
    : ICommandHandler<DeleteNoteCommand, string>
{

    public async Task<OneOf<string, Error>> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Note note = await notesModuleRepository
                .FindNotesWhere(f => f.Id == request.NoteId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given note doesn't exist.");

            string noteName = note.Name;
            notesModuleRepository.DeleteNote(note);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(noteName);
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}