using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateNoteLineCommandHandler(INotesModuleRepository notesModuleRepository)
    : ICommandHandler<UpdateNoteLineCommand, NoteLine>
{
    public async Task<OneOf<NoteLine, Error>> Handle(UpdateNoteLineCommand request, CancellationToken cancellationToken)
    {
        var line = notesModuleRepository.FindNoteLine(request.LineId) ?? throw new KeyNotFoundException("Line was not found");
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

        return new(line);
    }
}