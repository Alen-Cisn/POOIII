
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;
namespace Txt.Api.Controllers;

[ApiController]
[Route("Notes/{noteId:int}/Lines")]
[Authorize]
public class LinesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<NoteLineDto>> Get(int noteId, CancellationToken cancellationToken)
        => mediator.Send(new NoteLinesByNoteIdQuery(noteId: noteId), cancellationToken);

    [HttpPost]
    public async Task<ActionResult<NoteLineDto>> Post(int noteId, [FromBody] NoteLineDto noteLineDto)
        => (await mediator.Send(new CreateNoteLineCommand()
        {
            NoteId = noteId,
            Content = noteLineDto.Content,
            OrderIndex = noteLineDto.OrderIndex,
        })).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );

    [HttpPut]
    public async Task<ActionResult<NoteLineDto>> Put(int noteId, [FromBody] NoteLineDto noteLineDto)
        => (await mediator.Send(new UpdateNoteLineCommand()
        {
            NoteId = noteId,
            LineId = noteLineDto.Id,
            Content = noteLineDto.Content,
            OrderIndex = noteLineDto.OrderIndex,
        })).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );
}
