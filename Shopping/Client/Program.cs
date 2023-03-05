using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Shopping.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var api = configuration.GetValue<string>("Api");
    return new HttpClient { BaseAddress = new Uri(api) };
});
builder.Services.AddScoped<DialogService>();

await builder.Build().RunAsync();