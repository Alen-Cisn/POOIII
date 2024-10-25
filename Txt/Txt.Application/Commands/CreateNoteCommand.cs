using AutoMapper;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class CreateNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<CreateNoteCommand, NoteDto>
{

    public async Task<OneOf<NoteDto, Error>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            Name = request.Name,
            ParentId = request.FolderId,
            Lines = []
        };

        var entry = notesModuleRepository.CreateNote(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<NoteDto>(entry.Entity));
    }
}