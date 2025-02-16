using Duende.IdentityServer.Models;

namespace FlashApp.Infrastructure.Configuration;

public sealed class IdentityOptions
{
    public string AuthorizationEndpoint { get; init; }
    public string TokenEndpoint { get; init; }
    public string Audience { get; init; } 
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public bool UseHttps { get; init; } = true;
    public bool RequireHttpsMetadata { get; init; }
    public string ApiKey { get; init; } = "Flash";
}

public static class IdentityDefaults
{
    public const string LoginPath = "external/login";
    public const string LogoutPath = "external/logout";
    public const string CallbackPath = "external/callback";
    public const string LoginUrl = $"/api/{LoginPath}";
    public const string LogoutUrl = $"/api/{LogoutPath}";
    public const string CallbackUrl = $"/api/{CallbackPath}";

    public static class TestData
    {
        public static ICollection<Client> Clients =>
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

        public static ICollection<ApiScope> ApiScopes =>
        [
            new ApiScope("api1", "My API")
        ];

        public static ICollection<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];
    }
}
