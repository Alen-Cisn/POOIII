using System.Data;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore.Query;
using Txt.Ui.Helpers;
namespace Txt.Ui;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddTransient<AuthorizationHandler>();
        var clientBaseAddress = new Uri(builder.Configuration["apiurl"]
            ?? throw new NoNullAllowedException("apirul is null (reading from config file)"));

        builder.Services.AddHttpClient("Public.Txt.Api", client => client.BaseAddress = clientBaseAddress);

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthorizationHandler>();
        builder.Services
            .AddHttpClient("Txt.Api", client => client.BaseAddress = clientBaseAddress)
            .AddHttpMessageHandler<AuthorizationHandler>();

        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped<Helpers.AuthenticationStateProvider>();
        builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>(s => s.GetRequiredService<Helpers.AuthenticationStateProvider>());

        builder.Services.AddHttpClient<Helpers.AuthenticationStateProvider>(client => client.BaseAddress = clientBaseAddress);
        builder.Services.AddHttpClient<AuthorizationHandler>((sp, _) => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Public.Txt.Api"));

        builder.Services.AddLocalServices();

        await builder.Build().RunAsync();
    }
}