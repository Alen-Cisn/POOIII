
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;

namespace Txt.Ui.Services.Interfaces;

public interface INotesService
{
    public Task<Error?> UpdateNoteLineAsync(int id, int noteId, string content, int orderIndex);
    public Task<NoteDto?> GetNoteAsync(int id);
    public Task<Error?> DeleteNoteAsync(int id);
    public Task<Error?> DeleteNoteLineAsync(int noteId, int lineId);
    public Task<FolderDto?> GetRootFolderAsync();
    public Task<IEnumerable<NoteDto>> GetNotesByParentIdAsync(int parentId);
    public Task<IEnumerable<NoteLineDto>> GetNoteLinesByNoteIdAsync(int noteId);
    public Task<IEnumerable<FolderDto>> GetFoldersByParentIdAsync(int? parentId);
    public Task<Error?> UpdateNoteAsync(int id, string name, int parentId);
    public Task<Error?> CreateNoteAsync(string name, int parentId);
    public Task<Error?> UpdateFolderAsync(int id, string name, int? parentId);
    public Task<Error?> CreateFolderAsync(string name, int? parentId);
    public Task<Error?> DeleteFolderAsync(int id);
}
