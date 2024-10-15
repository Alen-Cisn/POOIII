using MediatR;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;

namespace Txt.Application.Commands;

public class CreateNoteCommandHandler(INotesRepository notesRepository)
    : IRequestHandler<CreateNoteCommand, int>
{
    public async Task<int> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            Description = request.Description,
        };

        var createdNote = await notesRepository.CreateAsync(note, cancellationToken);
        return createdNote.Id;
    }
}