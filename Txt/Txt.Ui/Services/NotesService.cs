using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;
using Txt.Ui.Services.HttpClients.Interfaces;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class NotesService(ITxtApiClientService clientService, ILogger<NotesService> logger) : INotesService
{
    private HttpClient HttpClient { get; init; } = clientService.HttpClient;

    private const string NotesEndpoint = "/notes";
    private const string FoldersEndpoint = "/folders";

    public async Task<Error?> UpdateNoteLineAsync(int id, int noteId, string content, int orderIndex)
    {
        string? error;
        try
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(new NoteLineDto()
                {
                    Id = id,
                    NoteId = noteId,
                    Content = content,
                    OrderIndex = orderIndex
                }),
                Encoding.UTF8,
                "application/json");
            var result = await HttpClient.PutAsync(NotesEndpoint + "/" + noteId + "/lines", httpContent);

            result.EnsureSuccessStatusCode();

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while updating note line: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while updating note line: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating note line: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<NoteDto?> GetNoteAsync(int id)
    {
        try
        {
            var result = await HttpClient.GetAsync(NotesEndpoint + "/" + id);

            result.EnsureSuccessStatusCode();

            return result.Content.ReadFromJsonAsync<NoteDto>().Result!;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching note: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching note: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching note: {Message}", ex.Message);
        }
        return null;
    }

    public async Task<FolderDto?> GetRootFolderAsync()
    {
        try
        {
            var result = await HttpClient.GetAsync(FoldersEndpoint + "/root");

            result.EnsureSuccessStatusCode();

            return result.Content.ReadFromJsonAsync<FolderDto>().Result!;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching note: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching note: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching note: {Message}", ex.Message);
        }
        return null;
    }

    public async Task<IEnumerable<NoteDto>> GetNotesByParentIdAsync(int parentId)
    {
        try
        {
            var result = await HttpClient.GetAsync(NotesEndpoint + "?parentId=" + parentId);

            result.EnsureSuccessStatusCode();

            return result.Content.ReadFromJsonAsync<IEnumerable<NoteDto>>().Result!;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching claims: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching claims: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching claims: {Message}", ex.Message);
        }
        return [];
    }

    public async Task<IEnumerable<NoteLineDto>> GetNoteLinesByNoteIdAsync(int noteId)
    {
        try
        {
            var result = await HttpClient.GetAsync(NotesEndpoint + "/" + noteId + "/lines");

            result.EnsureSuccessStatusCode();

            return result.Content.ReadFromJsonAsync<IEnumerable<NoteLineDto>>().Result!;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching claims: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching claims: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching claims: {Message}", ex.Message);
        }
        return [];
    }

    public async Task<IEnumerable<FolderDto>> GetFoldersByParentIdAsync(int? parentId)
    {
        try
        {
            var result = await HttpClient.GetAsync(FoldersEndpoint + "?parentId=" + parentId);

            result.EnsureSuccessStatusCode();

            return result.Content.ReadFromJsonAsync<IEnumerable<FolderDto>>().Result!;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching folders: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching folders: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching folders: {Message}", ex.Message);
        }
        return [];
    }

    public async Task<Error?> UpdateNoteAsync(int id, string name, int parentId)
    {
        string? error;
        try
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(new UpdateNoteCommand()
                {
                    NoteId = id,
                    Name = name,
                    ParentId = parentId
                }),
                Encoding.UTF8,
                "application/json");

            var result = await HttpClient.PutAsync(NotesEndpoint, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while updating note: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while updating note: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating note: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> CreateNoteAsync(string name, int parentId)
    {
        string? error;
        try
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(new CreateNoteCommand()
                {
                    Name = name,
                    ParentId = parentId
                }),
                Encoding.UTF8,
                "application/json");

            var result = await HttpClient.PostAsync(NotesEndpoint, httpContent);
            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while creating note: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while creating note: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while creating note: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> DeleteNoteAsync(int noteId)
    {
        string? error;
        try
        {
            var result = await HttpClient.DeleteAsync(NotesEndpoint + "/" + noteId);

            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while deleting note: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while deleting note: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while deleting note: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> DeleteNoteLineAsync(int noteId, int lineId)
    {
        string? error;
        try
        {
            var result = await HttpClient.DeleteAsync(NotesEndpoint + "/" + noteId + "/lines/" + lineId);

            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while deleting note line: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while deleting note line: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while deleting note line: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> DeleteFolderAsync(int folderId)
    {
        string? error;
        try
        {
            var result = await HttpClient.DeleteAsync(FoldersEndpoint + "/" + folderId);

            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while deleting folder: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while deleting folder: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while deleting folder: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> UpdateFolderAsync(int id, string name, int? parentId)
    {
        string? error;
        try
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(new UpdateFolderCommand()
                {
                    FolderId = id,
                    Name = name,
                    ParentId = parentId
                }),
                Encoding.UTF8,
                "application/json");

            var result = await HttpClient.PutAsync(FoldersEndpoint, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while updating folder: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while updating folder: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while updating folder: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public async Task<Error?> CreateFolderAsync(string name, int? parentId)
    {
        string? error;
        try
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(new CreateFolderCommand()
                {
                    Name = name,
                    ParentId = parentId
                }),
                Encoding.UTF8,
                "application/json");

            var result = await HttpClient.PostAsync(FoldersEndpoint, httpContent);
            if (!result.IsSuccessStatusCode)
            {
                return result.Content.ReadFromJsonAsync<Error>().Result;
            }

            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while creating folder: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while creating folder: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while creating folder: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }
}
