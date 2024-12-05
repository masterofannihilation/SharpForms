using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using SharpForms.Common.Enums;

namespace SharpForms.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(), new IdentityResources.Profile(),
            new("sharpforms_api", ["role", "email", "custom_claim"]),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[] { };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[] { };

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
            },
            RedirectUris = { "https://localhost:7143/authentication/login-callback" },
            RequireClientSecret = false,
        },
    ];
}
