
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;

namespace Txt.Ui.Services.Interfaces;

public interface INotesService
{
    public Task<Error?> UpdateNoteLineAsync(int id, int noteId, string content);
    public Task<NoteDto> GetNoteAsync(int id);
}
