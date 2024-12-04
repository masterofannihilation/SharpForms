using Duende.IdentityServer.Models;

namespace SharpForms.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[] { new IdentityResources.OpenId() };

    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new("sharpforms_api", ["role"]),
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client
        {
            ClientId = "api",
            ClientName = "API Client",
            AllowedGrantTypes =
            [
                GrantType.ClientCredentials,
                GrantType.ResourceOwnerPassword,
                GrantType.AuthorizationCode
            ],
            ClientSecrets = { new Secret("secret".Sha256()) },
            AllowedScopes = { "sharpforms_api", },
            RedirectUris = { "https://yourapp.com/callback" },
        },
    ];
}
