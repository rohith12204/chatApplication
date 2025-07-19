using BlazingChat.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization; // ✅ Add this

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ✅ Register local storage for JWT storage
builder.Services.AddBlazoredLocalStorage();

// ✅ Add auth support
builder.Services.AddAuthorizationCore();

// ✅ Set up HttpClient for API calls
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7189") // use your actual server URL
});

await builder.Build().RunAsync();
