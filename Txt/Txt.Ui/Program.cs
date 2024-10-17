using System.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
namespace Txt.Ui;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        var clientBaseAddress = new Uri(builder.Configuration["apiurl"]
            ?? throw new NoNullAllowedException("apirul is null (reading from config file)"));

        builder.Services
            .AddHttpClient("Txt.Api", client => client.BaseAddress = clientBaseAddress)
            .AddHttpMessageHandler(sp =>
            {
                //this is need when api is separated. https://code-maze.com/using-access-token-with-blazor-webassembly-httpclient/
                var handler = sp.GetService<AuthorizationMessageHandler>()!
                .ConfigureHandler(
                    authorizedUrls: [clientBaseAddress.ToString()],
                    scopes: ["Txt.Api"]
                );
                return handler;
            });
        builder.Services.AddScoped(
            sp => sp.GetService<IHttpClientFactory>()!.CreateClient("Txt.Api"));

        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("oidc", options.ProviderOptions);
        });

        builder.Services.AddHttpClient<PublicClient>("Public.Txt.Api", client => client.BaseAddress = clientBaseAddress);

        await builder.Build().RunAsync();
    }
}