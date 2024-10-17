using MediatR;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;

namespace Txt.Application.Commands;

public class CreateNoteCommandHandler(INotesRepository notesRepository)
    : IRequestHandler<CreateNoteCommand, Note>
{
    public async Task<Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            Description = request.Description,
        };

        var entry = notesRepository.Create(note);

        await notesRepository.SaveAsync(cancellationToken);

        return entry.Entity;
    }
}