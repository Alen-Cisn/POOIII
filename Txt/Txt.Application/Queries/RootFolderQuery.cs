using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Dtos;
using Txt.Shared.Queries;

namespace Txt.Application.Queries;

public class RootFolderQueryHandler(INotesModuleRepository notesModuleRepository, IMapper mapper)
    : IRequestHandler<RootFolderQuery, FolderDto>
{
    public async Task<FolderDto> Handle(RootFolderQuery request, CancellationToken cancellationToken)
    {
        Stack<FolderDto> stack = new();
        List<FolderDto> foldersInRoot = mapper.Map<List<FolderDto>>(await notesModuleRepository.FindFoldersWhere(Folder =>
            Folder.ParentId == null
        ).ToListAsync(cancellationToken: cancellationToken));

        foreach (var folder in foldersInRoot)
        {
            stack.Push(folder);
        }

        while (stack.Count != 0)
        {
            FolderDto currentFolder = stack.Pop();

            List<FolderDto> childFolders = mapper.Map<List<FolderDto>>(await notesModuleRepository.FindFoldersWhere(Folder =>
                Folder.ParentId == currentFolder.Id
            ).ToListAsync(cancellationToken: cancellationToken));
            currentFolder.ChildrenFolders = childFolders;

            List<NoteDto> childNotes = mapper.Map<List<NoteDto>>(await notesModuleRepository.FindNotesWhere(Folder =>
                Folder.ParentId == currentFolder.Id
            ).ToListAsync(cancellationToken: cancellationToken));
            currentFolder.ChildrenFolders = childFolders;
            currentFolder.ChildrenNotes = childNotes;

            foreach (var childFolder in childFolders)
            {
                stack.Push(childFolder);
            }
        }

        FolderDto rootFolder = new()
        {
            Name = "Root",
            Path = "/",
            ChildrenFolders = foldersInRoot
        };

        return rootFolder;
    }
}