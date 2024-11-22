using AutoMapper;
using Microsoft.Extensions.Logging;
using Txt.Application.Commands.Interfaces;
using Txt.Application.PipelineBehaviors;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateNoteLineCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper, ILogger<UpdateNoteLineCommandHandler> logger)
    : ICommandHandler<UpdateNoteLineCommand, NoteLineDto>
{
    public async Task<OneOf<NoteLineDto, Error>> Handle(UpdateNoteLineCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var line = notesModuleRepository.FindNoteLine(request.LineId) ?? throw new NotFoundException("Line was not found");
            if (request.Content != null)
            {
                line.Content = request.Content;
            }
            if (request.OrderIndex.HasValue)
            {
                line.OrderIndex = request.OrderIndex.Value;
            }

            notesModuleRepository.UpdateNoteLine(line);

            await notesModuleRepository.SaveAsync(cancellationToken);

            return new(mapper.Map<NoteLineDto>(line));
        }
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }
}