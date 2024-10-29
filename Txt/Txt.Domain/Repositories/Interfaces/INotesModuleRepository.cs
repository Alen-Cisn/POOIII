using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Txt.Domain.Entities;

namespace Txt.Domain.Repositories.Interfaces;

public interface INotesModuleRepository : ISavableRepository
{
    IQueryable<Note> FindAllNotes();
    IQueryable<Note> FindNotesWhere(Expression<Func<Note, bool>> expression);
    Task<EntityEntry<Note>> CreateNoteAsync(Note note, CancellationToken cancellationToken = default);
    EntityEntry<Note> CreateNote(Note note);
    void UpdateNote(Note note);
    void DeleteNote(Note note);

    NoteLine? FindNoteLine(int noteLineId);
    IQueryable<NoteLine> FindAllNoteLines(Note note);
    IQueryable<NoteLine> FindAllNoteLines(int noteId);
    Task<EntityEntry<NoteLine>> CreateNoteLineAsync(NoteLine noteLine, CancellationToken cancellationToken = default);
    EntityEntry<NoteLine> CreateNoteLine(NoteLine noteLine);
    void UpdateNoteLine(NoteLine noteLine);
    void DeleteNoteLine(NoteLine noteLine);

    IQueryable<Folder> FindFoldersWhere(Expression<Func<Folder, bool>> expression);
    EntityEntry<Folder> CreateFolder(Folder folder);
    void UpdateFolder(Folder folder);
}