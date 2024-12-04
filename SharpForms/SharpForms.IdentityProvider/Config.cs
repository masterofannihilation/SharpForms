using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using SharpForms.Common.Enums;

namespace SharpForms.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new("roles", ["role"])
    ];
    public static IEnumerable<ApiResource> ApiResources =>
    [
        new ApiResource("sharpforms_api", "SharpForms API")
        {
            Scopes = { "sharpforms_api" },
            UserClaims = { "role" } 
        }
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
                GrantType.AuthorizationCode
            ],
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "sharpforms_api",
                "roles",
            },
            RedirectUris = { "https://localhost:7143/authentication/login-callback" },
            RequireClientSecret = false,
        },
    ];
}
