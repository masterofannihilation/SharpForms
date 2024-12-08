using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using SharpForms.Common.Enums;

namespace SharpForms.IdentityProvider;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources
    {
get
    {
        var profileIdentityResources = new IdentityResources.Profile();
        profileIdentityResources.UserClaims.Add("username");
        profileIdentityResources.UserClaims.Add("role");
        profileIdentityResources.UserClaims.Add("userid");

        return
        [
            new IdentityResources.OpenId(),
            profileIdentityResources
        ];
    }
}

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("sharpforms_api", "SharpForms API")
            {
                Scopes = { "sharpforms_api" },
                UserClaims = { "name", "role", "userid" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("sharpforms_api", "Access to SharpForms API")
            {
                UserClaims = { "name", "role", "userid" }
            }
        };

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
            PostLogoutRedirectUris = { "https://localhost:7143/" },
            RequireClientSecret = false,
            RequirePkce = true,
        },
    ];
}
