using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Serilog;

namespace SharpForms.IdentityProvider;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        var redirectUri = builder.Configuration.GetValue<string>("RedirectUri");
        
        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(redirectUri!))
            .AddTestUsers(Users.GetUsers());

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        app.UseCors(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
