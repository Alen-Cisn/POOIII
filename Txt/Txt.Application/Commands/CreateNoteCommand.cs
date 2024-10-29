using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class CreateNoteCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<CreateNoteCommand, NoteDto>
{

    public async Task<OneOf<NoteDto, Error>> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        Folder folder = await notesModuleRepository
            .FindFoldersWhere(f => f.Id == request.FolderId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new ValidationException("Given parent folder doesn't exist.");

        Note note = new()
        {
            Name = request.Name,
            ParentId = request.FolderId,
            Lines = [],
            Path = folder.Path + "/" + request.Name
        };

        EntityEntry<Note> entry = notesModuleRepository.CreateNote(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<NoteDto>(entry.Entity));
    }
}