
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
public class FoldersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of folders based on the optional parent folder ID.
    /// </summary>
    /// <param name="parentId">The ID of the parent folder to filter the folders. If null, retrieves all folders.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of FolderDto objects.</returns>
    [HttpGet]
    public Task<List<FolderDto>> Get([FromQuery] int? parentId, CancellationToken cancellationToken)
        => mediator.Send(new FoldersByParentFolderIdQuery(folderId: parentId), cancellationToken);

    /// <summary>
    /// Retrieves the root folder.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a FolderDto object.</returns>
    [HttpGet("root")]
    public Task<FolderDto> GetRoot(CancellationToken cancellationToken)
        => mediator.Send(new RootFolderQuery(), cancellationToken);

    /// <summary>
    /// Retrieves a specific folder by its ID.
    /// </summary>
    /// <param name="id">The ID of the folder to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a FolderDto object.</returns>
    [HttpGet(@"{id:int}")]
    public Task<FolderDto> GetById(int id, CancellationToken cancellationToken)
        => mediator.Send(new FolderByIdQuery(id), cancellationToken);

    /// <summary>
    /// Creates a new folder.
    /// </summary>
    /// <param name="command">The command containing the data of the folder to be created.</param>
    /// <returns>
    /// A 200 OK response containing the newly created <see cref="FolderDto"/>,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<FolderDto>> Post([FromBody] CreateFolderCommand command)
        => (await mediator.Send(command)).Match<ActionResult<FolderDto>>(
            folder => Ok(folder),
            error => BadRequest(error)
            );

    /// <summary>
    /// Updates an existing folder with the provided data.
    /// </summary>
    /// <param name="command">The command containing the data to update the folder.</param>
    /// <returns>
    /// A 200 OK response containing the updated <see cref="FolderDto"/>,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
    [HttpPut]
    public async Task<ActionResult<FolderDto>> Put([FromBody] UpdateFolderCommand command)
        => (await mediator.Send(command)).Match<ActionResult<FolderDto>>(
            folder => Ok(folder),
            error => BadRequest(error)
            );

    /// <summary>
    /// Deletes a folder based on the provided command.
    /// </summary>
    /// <param name="command">The command containing the ID of the folder to be deleted.</param>
    /// <returns>
    /// A 200 OK response containing the name of the deleted folder as a string,
    /// or a 400 Bad Request response containing error information.
    /// </returns>
    [HttpDelete(@"{id:int}")]
    public async Task<ActionResult<string>> Delete(int id)
        => (await mediator.Send(new DeleteFolderCommand() { FolderId = id })).Match<ActionResult<string>>(
            folderName => Ok(folderName),
            error => BadRequest(error)
            );
}
