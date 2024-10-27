using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class FoldersByParentFolderIdQuery(int? folderId) : IRequest<List<FolderDto>>
{
    public int? FolderId { get; set; } = folderId;
}