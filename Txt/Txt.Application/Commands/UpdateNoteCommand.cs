using AutoMapper;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<UpdateNoteCommand, NoteDto>
{
    public async Task<OneOf<NoteDto, Error>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = new Note
        {
            Id = request.NoteId,
            Name = request.Name,
            ParentId = request.FolderId,
            Lines = []
        };

        note.Lines = notesModuleRepository.FindAllNoteLines(note);

        notesModuleRepository.UpdateNote(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<NoteDto>(note));
    }
}