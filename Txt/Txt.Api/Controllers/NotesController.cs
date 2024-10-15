
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
    {
        return mediator.Send(new NoteQuery(), cancellationToken);
    }

    [HttpPost]
    public Task<int> Post([FromBody] CreateNoteCommand command)
    {
        Console.WriteLine("Acá se llega");
        return mediator.Send(command);
    }
}
