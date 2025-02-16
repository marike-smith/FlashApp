using System.Text.Json.Serialization;

namespace FlashApp.Infrastructure.Authentication.Models;

/// <summary>
/// Authorization token model.
/// </summary>
public sealed class AuthorizationToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = string.Empty;
}
