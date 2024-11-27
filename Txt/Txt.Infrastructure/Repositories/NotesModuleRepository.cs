using System.Linq.Expressions;
using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public class NotesModuleRepository(
    ApplicationDbContext context,
    ICurrentUserService currentUserService
    )
    : INotesModuleRepository
{
    private NotesRepository NotesRepository { get; set; } = new NotesRepository(context, currentUserService);
    private NoteLinesRepository NoteLinesRepository { get; set; } = new NoteLinesRepository(context, currentUserService);
    private FoldersRepository FoldersRepository { get; set; } = new FoldersRepository(context, currentUserService);

    public Note CreateNote(Note note)
    {
        if (!note.Lines.Any())
        {
            note.Lines = [new NoteLine() {
                Content = "",
                CreatedById = currentUserService.UserId,
                CreatedOn = DateTime.Now,
            }];
        }

        return NotesRepository.Create(note);
    }

    public Task<Note> CreateNoteAsync(Note note, CancellationToken cancellationToken = default)
    {
        if (!note.Lines.Any())
        {
            note.Lines = [new NoteLine() {
                Content = "",
                CreatedById = currentUserService.UserId,
                CreatedOn = DateTime.Now,
            }];
        }

        return NotesRepository.CreateAsync(note, cancellationToken);
    }

    public NoteLine CreateNoteLine(NoteLine noteLine)
        => NoteLinesRepository.Create(noteLine);

    public Task<NoteLine> CreateNoteLineAsync(NoteLine noteLine, CancellationToken cancellationToken = default)
        => NoteLinesRepository.CreateAsync(noteLine, cancellationToken);

    public void DeleteNote(Note note)
    {
        NoteLinesRepository.DeleteRange(NoteLinesRepository.FindWhere(line => line.NoteId == note.Id));
        NotesRepository.Delete(note);
    }

    public void DeleteFolder(Folder folder)
    {
        var childrenNotes = NotesRepository.FindWhere(note => note.ParentId == folder.Id);
        NotesRepository.DeleteRange(childrenNotes);
        var childrenFolders = FoldersRepository.FindWhere(f => f.ParentId == folder.Id);
        foreach (var childFolder in childrenFolders)
        {
            DeleteFolder(childFolder);
        }
        FoldersRepository.Delete(folder);
    }

    public void DeleteNoteLine(NoteLine noteLine)
        => NoteLinesRepository.Delete(noteLine);

    public IQueryable<NoteLine> FindAllNoteLines(Note note)
        => NoteLinesRepository.FindWhere(line => line.NoteId == note.Id);

    public IQueryable<NoteLine> FindAllNoteLines(int noteId)
        => NoteLinesRepository.FindWhere(line => line.NoteId == noteId);
    public NoteLine? FindNoteLine(int noteLineId)
        => NoteLinesRepository.FindWhere(line => line.Id == noteLineId).FirstOrDefault();

    public IQueryable<Note> FindAllNotes()
        => NotesRepository.FindAll();

    public IQueryable<Note> FindNotesWhere(Expression<Func<Note, bool>> expression)
        => NotesRepository.FindWhere(expression);

    public IQueryable<Folder> FindFoldersWhere(Expression<Func<Folder, bool>> expression)
        => FoldersRepository.FindWhere(expression);

    public Folder CreateFolder(Folder folder)
        => FoldersRepository.Create(folder);

    public void UpdateFolder(Folder folder)
    {

        FoldersRepository.Update(folder);
        Stack<Folder> stack = new();

        stack.Push(folder);

        while (stack.Count != 0)
        {
            Folder currentFolder = stack.Pop();
            Note[] notes = [.. NotesRepository.FindWhere(note => note.ParentId == currentFolder.Id)];
            foreach (var note in notes)
            {
                note.Path = currentFolder.Path + "/" + note.Name;
            }

            NotesRepository.UpdateRange(notes);

            Folder[] childFolders = [.. FoldersRepository.FindWhere(f => f.ParentId == currentFolder.Id)];
            foreach (var childFolder in childFolders)
            {
                childFolder.Path = currentFolder.Path + "/" + childFolder.Name;
                stack.Push(childFolder);
            }

            FoldersRepository.UpdateRange(childFolders);
        }

    }

    public Task<int> SaveAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public void UpdateNote(Note note)
        => NotesRepository.Update(note);

    public void UpdateNoteLine(NoteLine noteLine)
        => NoteLinesRepository.Update(noteLine);
}