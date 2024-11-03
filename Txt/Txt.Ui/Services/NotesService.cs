using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    private const string Endpoint = "/notes";

    public async Task<AccountInformation?> Get()
    {
        try
        {
            return await HttpClient.GetFromJsonAsync<AccountInformation>(Endpoint);

        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching account information: {Message}", httpEx.Message);
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching account information: {Message}", jsonEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching account information: {Message}", ex.Message);
        }
        return null;
    }

    public async Task<IEnumerable<ClaimDto>?> GetClaims()
    {
        try
        {
            return await HttpClient.GetFromJsonAsync<IEnumerable<ClaimDto>>(Endpoint + "/claims");
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
        return null;
    }

    public async Task<Error?> UpdateNoteLineAsync(int id, int noteId, string content)
    {
        string? error = null;
        try
        {
            // HttpContent httpContent = new JsonContent.Create<UpdateNoteLineCommand>(new UpdateNoteLineCommand()
            // {
            //     LineId = id,
            //     NoteId = noteId,
            //     Content = content
            // }, MediaTypeHeaderValue.Parse);
            // return await HttpClient.PutAsync(Endpoint + "/claims", );
            return null;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request error while fetching claims: {Message}", httpEx.Message);
            error = "Http error";
        }
        catch (JsonException jsonEx)
        {
            logger.LogError(jsonEx, "JSON deserialization error while fetching claims: {Message}", jsonEx.Message);
            error = "JSon parsing error.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching claims: {Message}", ex.Message);
            error = "Unexpected error";
        }
        return new()
        {
            Details = error
        };
    }

    public Task<NoteDto> GetNoteAsync(int id)
    {
        throw new NotImplementedException();
    }
}
