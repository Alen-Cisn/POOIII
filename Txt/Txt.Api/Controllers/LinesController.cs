
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;
namespace Txt.Api.Controllers;

[ApiController]
[Route("notes/{noteId:int}/lines")]
[Authorize]
public class LinesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<NoteLineDto>> Get(int noteId, CancellationToken cancellationToken)
        => mediator.Send(new NoteLineQuery(noteId: noteId), cancellationToken);

    [HttpGet("{id:int}")]
    public Task<List<NoteLineDto>> Get(int noteId, int id, CancellationToken cancellationToken)
        => mediator.Send(new NoteLineQuery(noteId: noteId), cancellationToken);

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
