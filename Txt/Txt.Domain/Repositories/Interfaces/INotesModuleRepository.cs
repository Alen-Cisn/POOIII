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

    IQueryable<NoteLine> FindAllNoteLines(Note note);
    NoteLine? FindNoteLine(int noteLineId);
    Task<EntityEntry<NoteLine>> CreateNoteLineAsync(NoteLine noteLine, CancellationToken cancellationToken = default);
    EntityEntry<NoteLine> CreateNoteLine(NoteLine noteLine);
    void UpdateNoteLine(NoteLine noteLine);
    void DeleteNoteLine(NoteLine noteLine);
}