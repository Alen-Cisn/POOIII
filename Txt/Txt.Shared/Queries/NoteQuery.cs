using MediatR;
using Txt.Domain.Entities;

namespace Txt.Shared.Queries;

public class NoteQuery(int? id = null, int? folderId = null) : IRequest<List<Note>>
{
    public int? Id { get; set; } = id;
    public int? FolderId { get; set; } = folderId;
}