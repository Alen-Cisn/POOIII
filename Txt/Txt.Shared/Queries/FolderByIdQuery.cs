using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class FolderByIdQuery(int folderId) : IRequest<FolderDto>
{
    public int FolderId { get; set; } = folderId;
}