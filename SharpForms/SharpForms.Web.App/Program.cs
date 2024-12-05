using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SharpForms.Web.App;
using SharpForms.Web.BL.Installers;
using SharpForms.Web.BL.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazorBootstrap();

builder.Configuration.AddJsonFile("appsettings.json");

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler(serviceProvider
        => serviceProvider?.GetService<AuthorizationMessageHandler>()
            ?.ConfigureHandler(
                authorizedUrls: new[] { apiBaseUrl },
                scopes: new[] { "sharpforms_api", }));
builder.Services.AddScoped<HttpClient>(serviceProvider =>
    serviceProvider.GetService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("IdentityServer", options.ProviderOptions);
    var configurationSection = builder.Configuration.GetSection("IdentityServer");
    var authority = configurationSection["Authority"];

    options.ProviderOptions.DefaultScopes.Add("sharpforms_api");
    options.UserOptions.RoleClaim = "role";
});

builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl!);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
