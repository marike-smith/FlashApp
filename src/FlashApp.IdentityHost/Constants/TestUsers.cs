using Duende.IdentityServer.Models;

namespace FlashApp.IdentityHost.Constants;

internal static class TestUsers
{
    internal static ICollection<Client> Clients =>
    [
        new Client
        {
            ClientId = "client-app",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            RedirectUris = new List<string> { "https://localhost:5002/signin-oidc" },
            PostLogoutRedirectUris = new List<string> { "https://localhost:5002/signout-callback-oidc" },
            AllowedScopes = new List<string> { "openid", "profile", "api1" }
        }
    ];

    internal static ICollection<ApiScope> ApiScopes =>
    [
        new ApiScope("api1", "My API")
    ];

    internal static ICollection<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    ];
}