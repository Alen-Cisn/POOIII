using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NoteQuery(int? id = null, int? folderId = null) : IRequest<List<NoteDto>>
{
    public int? Id { get; set; } = id;
    public int? FolderId { get; set; } = folderId;
}