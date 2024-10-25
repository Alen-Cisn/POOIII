
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Domain.Entities;
using Txt.Shared.Commands;
using Txt.Shared.Queries;
namespace Txt.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class NotesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<Note>> Get([FromQuery] int? folderId, CancellationToken cancellationToken)
        => mediator.Send(new NoteQuery(folderId: folderId), cancellationToken);

    [HttpGet(@"{id:int}")]
    public Task<List<Note>> GetById(int id, CancellationToken cancellationToken)
        => mediator.Send(new NoteQuery(id), cancellationToken);

    [HttpPost]
    public async Task<ActionResult<Note>> Post([FromBody] CreateNoteCommand command)
        => (await mediator.Send(command)).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );

    [HttpPut]
    public async Task<ActionResult<Note>> Put([FromBody] UpdateNoteCommand command)
        => (await mediator.Send(command)).Match<ActionResult>(
            Ok,
            error => BadRequest()
            );
}
