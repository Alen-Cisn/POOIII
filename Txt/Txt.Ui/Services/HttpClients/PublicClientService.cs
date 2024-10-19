using Txt.Ui.Services.HttpClients.Interfaces;

namespace Txt.Ui.Services.HttpClients;

public class PublicClientService(IHttpClientFactory httpClientFactory) : IPublicClientService
{
    public required HttpClient HttpClient { get; init; } = httpClientFactory.CreateClient("Public.Txt.Api");
}
