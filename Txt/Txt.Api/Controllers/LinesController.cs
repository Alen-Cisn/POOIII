
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Domain.Entities;
using Txt.Shared.Commands;
using Txt.Shared.Queries;
namespace Txt.Api.Controllers;

[ApiController]
[Route("notes/lines")]
[Authorize]
public class LinesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<NoteLine>> Get(int noteId, CancellationToken cancellationToken)
        => mediator.Send(new NoteLineQuery(noteId: noteId), cancellationToken);

    [HttpPost]
    public async Task<ActionResult<Note>> Post([FromBody] CreateNoteLineCommand command)
        => (await mediator.Send(command)).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );

    [HttpPut]
    public async Task<ActionResult<Note>> Put([FromBody] UpdateNoteLineCommand command)
        => (await mediator.Send(command)).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );
}
