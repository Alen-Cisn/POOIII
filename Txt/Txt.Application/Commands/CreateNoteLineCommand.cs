using AutoMapper;
using Txt.Application.Commands.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands;

public class CreateNoteLineCommandHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : ICommandHandler<CreateNoteLineCommand, NoteLineDto>
{
    public async Task<OneOf<NoteLineDto, Error>> Handle(CreateNoteLineCommand request, CancellationToken cancellationToken)
    {
        var note = new NoteLine
        {
            NoteId = request.NoteId,
            Content = request.Content,
            OrderIndex = request.OrderIndex,
        };

        var entry = notesModuleRepository.CreateNoteLine(note);

        await notesModuleRepository.SaveAsync(cancellationToken);

        return new(mapper.Map<NoteLineDto>(entry.Entity));
    }
}