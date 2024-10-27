using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class NotesByFolderIdQuery(int folderId) : IRequest<List<NoteDto>>
{
    public int FolderId { get; set; } = folderId;
}