
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;
namespace Txt.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class NotesController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public Task<List<NoteDto>> Get([FromQuery] int folderId, CancellationToken cancellationToken)
        => mediator.Send(new NotesByFolderIdQuery(folderId: folderId), cancellationToken);

    [HttpGet(@"{id:int}")]
    public Task<NoteDto> GetById(int id, CancellationToken cancellationToken)
        => mediator.Send(new NoteByIdQuery(id), cancellationToken);

    [HttpPost]
    public async Task<ActionResult<NoteDto>> Post([FromBody] CreateNoteCommand command)
        => (await mediator.Send(command)).Match<ActionResult<NoteDto>>(
            note => Ok(note),
            error => BadRequest()
            );

    [HttpPut]
    public async Task<ActionResult<NoteDto>> Put([FromBody] UpdateNoteCommand command)
        => (await mediator.Send(command)).Match<ActionResult<NoteDto>>(
            note => Ok(note),
            error => BadRequest()
            );
}
