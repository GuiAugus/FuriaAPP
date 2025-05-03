using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using FuriaAPP.Client.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using FuriaAPP.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ServerAPI", client => 
{
    client.BaseAddress = new Uri("http://localhost:5101/");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<CustomAuthorizationHandler>();

builder.Services.AddScoped(sp => 
    new HttpClient { 
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
    });

builder.Services.AddScoped<CustomAuthorizationHandler>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => 
    provider.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddLogging(logging => 
{
#if DEBUG
    logging.SetMinimumLevel(LogLevel.Debug);
#else
    logging.SetMinimumLevel(LogLevel.Warning);
#endif
    logging.AddFilter("Microsoft", LogLevel.Warning);
    logging.AddFilter("System", LogLevel.Warning);
});

try
{
    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro durante a inicialização: {ex}");
}