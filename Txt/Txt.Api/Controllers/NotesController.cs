
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
public class NotesController(ILogger<NotesController> logger, IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<Note>> Get(CancellationToken cancellationToken)
        => mediator.Send(new NoteQuery(), cancellationToken);

    [HttpPost]
    public Task<Note> Post([FromBody] CreateNoteCommand command)
        => mediator.Send(command);
}
