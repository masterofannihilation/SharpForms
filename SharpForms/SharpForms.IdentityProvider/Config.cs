using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace SharpForms.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ];

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new("sharpforms_api", ["role"]),
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client
        {
            ClientId = "api",
            ClientName = "SharpForms API Client",
            AllowedGrantTypes =
            [
                // GrantType.ClientCredentials,
                // GrantType.ResourceOwnerPassword,
                GrantType.AuthorizationCode
            ],
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "sharpforms_api",
            },
            RedirectUris = { "https://localhost:7143/authentication/login-callback" },
            RequireClientSecret = false,
        },
        // new Client
        // {
        //     ClientId = "blazor-client",
        //     ClientName = "Blazor WebAssembly App",
        //     AllowedGrantTypes = GrantTypes.Code,
        //     RequireClientSecret = false, // For Blazor WebAssembly
        //     RedirectUris = { "https://localhost:7143/authentication/login-callback" },
        //     PostLogoutRedirectUris = { "https://localhost:7143/authentication/logout-callback" },
        //     AllowedCorsOrigins = { "https://localhost:7143" },
        //     AllowedScopes = { "openid", "profile", "sharpforms_api" }, // Ensure scopes match API
        //     EnableLocalLogin = true
        // }
    ];
}
