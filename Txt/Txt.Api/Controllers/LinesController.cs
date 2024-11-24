
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

    /// <summary>
    /// Retrieves a list of lines within a specified note.
    /// </summary>
    /// <param name="noteId">The ID of the note to retrieve lines from.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of NoteLineDto objects.</returns>
    [HttpGet]
    public Task<List<NoteLineDto>> Get(int noteId, CancellationToken cancellationToken)
        => mediator.Send(new NoteLinesByNoteIdQuery(noteId: noteId), cancellationToken);


    /// <summary>
    /// Creates a new line within a specified note.
    /// </summary>
    /// <param name="noteId">The ID of the note to add the line to.</param>
    /// <param name="noteLineDto">The data transfer object containing line details.</param>
    /// <returns>
    /// A 200 OK response containing the newly created <see cref="NoteLineDto"/>,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<NoteLineDto>> Post(int noteId, [FromBody] NoteLineDto noteLineDto)
        => (await mediator.Send(new CreateNoteLineCommand()
        {
            NoteId = noteId,
            Content = noteLineDto.Content,
            OrderIndex = noteLineDto.OrderIndex,
        })).Match<ActionResult>(
            Ok,

            BadRequest
            );


    /// <summary>
    /// Updates an existing line within a specified note.
    /// </summary>
    /// <param name="noteId">The ID of the note to update the line in.</param>
    /// <param name="noteLineDto">The data transfer object containing line details.</param>
    /// <returns>
    /// A 200 OK response containing the updated <see cref="NoteLineDto"/>,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
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
            BadRequest
            );

    /// <summary>
    /// Deletes a specific line within a specified note.
    /// </summary>
    /// <param name="noteId">The ID of the note from which the line will be deleted.</param>
    /// <param name="lineId">The ID of the line to delete.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns>
    /// A 200 OK response containing a success message as a string,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
    [HttpDelete("{lineId:int}")]
    public async Task<ActionResult<string>> Delete(int noteId, int lineId, CancellationToken cancellationToken)
        => (await mediator.Send(new DeleteNoteLineCommand() { NoteId = noteId, LineId = lineId }, cancellationToken))
            .Match<ActionResult<string>>(
            successMessage => Ok(successMessage),
            error => BadRequest(error)
            );
}
