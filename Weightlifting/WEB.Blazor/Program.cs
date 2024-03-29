using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB.Blazor;
using WEB.Blazor.Services;
using WEB.Blazor.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateService>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthenticationStateService>());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

string apiBaseAddress = builder.Configuration["WeightliftingAPIAddress"];

if (string.IsNullOrWhiteSpace(apiBaseAddress))
    throw new InvalidOperationException("WeightliftingAPIAddress missing from appsettings file.");

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddHttpClient<IAthleteService, AthleteService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

builder.Services.AddHttpClient<ICoachService, CoachService>(client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});


builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
