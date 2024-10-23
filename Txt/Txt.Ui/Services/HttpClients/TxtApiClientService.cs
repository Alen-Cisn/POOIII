using Txt.Ui.Services.HttpClients.Interfaces;

namespace Txt.Ui.Services.HttpClients;

public class TxtApiClientService(IHttpClientFactory httpClientFactory) : ITxtApiClientService
{
    public required HttpClient HttpClient { get; init; } = httpClientFactory.CreateClient("Txt.Api");
}
