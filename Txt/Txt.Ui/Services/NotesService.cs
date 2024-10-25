using System.Net.Http.Json;
using System.Text.Json;
using Txt.Shared.Dtos;
using Txt.Ui.Services.HttpClients.Interfaces;
using Txt.Ui.Services.Interfaces;

namespace Txt.Ui.Services;

public class NotesService(ITxtApiClientService clientService, ILogger<NotesService> logger) : INotesService
{
    private HttpClient HttpClient { get; init; } = clientService.HttpClient;

    private const string Endpoint = "/account";

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
}
