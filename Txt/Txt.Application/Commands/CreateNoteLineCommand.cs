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

public class CreateNoteLineCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<CreateNoteLineCommandHandler> logger)
    : ICommandHandler<CreateNoteLineCommand, NoteLineDto>
{
    public async Task<OneOf<NoteLineDto, Error>> Handle(CreateNoteLineCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var note = await notesModuleRepository
                .FindNotesWhere(note => note.Id == request.NoteId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Note not found.");

            var noteLine = new NoteLine
            {
                NoteId = request.NoteId,
                Content = request.Content,
                OrderIndex = request.OrderIndex,
            };

            notesModuleRepository.FindAllNoteLines(note).Where(nl => nl.OrderIndex >= noteLine.OrderIndex).ToList().ForEach(nl => nl.OrderIndex++);

            var entity = notesModuleRepository.CreateNoteLine(noteLine);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(mapper.Map<NoteLineDto>(entity));
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}