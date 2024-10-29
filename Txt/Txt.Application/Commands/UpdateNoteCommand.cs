using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class UpdateNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<UpdateNoteCommand, NoteDto>
{
    public async Task<OneOf<NoteDto, Error>> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        Folder folder = await notesModuleRepository
            .FindFoldersWhere(f => f.Id == request.FolderId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ValidationException("Given parent folder doesn't exist.");

        var note = new Note
        {
            Id = request.NoteId,
            Name = request.Name,
            ParentId = request.FolderId,
            Lines = [],
            Path = folder.Path + "/" + request.Name
        };

        note.Lines = notesModuleRepository.FindAllNoteLines(note);

        notesModuleRepository.UpdateNote(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<NoteDto>(note));
    }
}