using System.Data;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Txt.Ui.Helpers;
using Microsoft.Extensions.DependencyInjection;
namespace Txt.Ui;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var clientBaseAddress = new Uri(builder.Configuration["apiurl"]
            ?? throw new NoNullAllowedException("apiurl is null (reading from config file)"));

        builder.Services.AddHttpClient("Public.Txt.Api", client => client.BaseAddress = clientBaseAddress);

        builder.Services.AddAuthorizationCore();
        builder.Services.AddTransient<AuthorizationHandler>();
        builder.Services
            .AddHttpClient("Txt.Api", client =>
            {
                client.BaseAddress = clientBaseAddress;

            })
            .ConfigureAdditionalHttpMessageHandlers((dh, sp) =>
            {
                dh.Add(sp.GetRequiredService<AuthorizationHandler>());
            });
        builder.Services.AddBlazoredLocalStorage();

        builder.Services.AddScoped<AuthenticationStateProvider>();
        builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, AuthenticationStateProvider>();

        builder.Services.AddHttpClient<AuthenticationStateProvider>(client => client.BaseAddress = clientBaseAddress);
        builder.Services.AddMudServices(cfg =>
        {
            cfg.SnackbarConfiguration.ShowTransitionDuration = 200;
        });

        builder.Services.AddLocalServices();
        var app = builder.Build();


        await app.RunAsync();
    }
}