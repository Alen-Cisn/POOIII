using Txt.Shared.Commands.Interfaces;

namespace Txt.Shared.Commands;

public class DeleteFolderCommand() : ICommand<string>
{
    public required int FolderId { get; set; }
}