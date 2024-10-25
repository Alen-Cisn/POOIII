using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class CreateNoteLineCommandHandler(INotesModuleRepository notesModuleRepository)
    : ICommandHandler<CreateNoteLineCommand, NoteLine>
{
    public async Task<OneOf<NoteLine, Error>> Handle(CreateNoteLineCommand request, CancellationToken cancellationToken)
    {
        var note = new NoteLine
        {
            NoteId = request.NoteId,
            Content = request.Content,
            OrderIndex = request.OrderIndex,
        };

        var entry = notesModuleRepository.CreateNoteLine(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(entry.Entity);
    }
}