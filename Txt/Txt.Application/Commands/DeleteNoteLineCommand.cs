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

public class DeleteNoteLineCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<DeleteNoteLineCommandHandler> logger)
    : ICommandHandler<DeleteNoteLineCommand, string>
{
    public async Task<OneOf<string, Error>> Handle(DeleteNoteLineCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Note note = await notesModuleRepository
                .FindNotesWhere(f => f.Id == request.NoteId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given note doesn't exist.");

            NoteLine noteLine = await notesModuleRepository
                .FindAllNoteLines(note)
                .Where(nl => nl.Id == request.LineId)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new ValidationException("Given note line doesn't exist.");

            notesModuleRepository.DeleteNoteLine(noteLine);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new("Note line deleted.");
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}