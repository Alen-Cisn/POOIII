using System.ComponentModel.DataAnnotations;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.Dtos;

namespace Txt.Shared.Commands;

public class CreateFolderCommand : ICommand<FolderDto>
{
    [Length(1, 180, ErrorMessage = "Name can't be empty.")]
    public required string Name { get; set; }
    public int? ParentId { get; set; }
}