using System.ComponentModel.DataAnnotations;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Shared.Commands;

public class UpdateFolderCommand : ICommand<FolderDto>
{
    public required int FolderId { get; set; }
    [Length(1, 180, ErrorMessage = "Name can't be empty.")]
    public required string Name { get; set; }
    public int? ParentId { get; set; }
}