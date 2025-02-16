namespace FlashApp.Infrastructure.Configuration;
public sealed class JwtOptions
{
    public string Audience { get; init; } = string.Empty;
    public string MetadataUrl { get; init; } = string.Empty;
    public bool RequireHttpsMetadata { get; init; }
    public string ValidIssuer { get; init; } = string.Empty;
}
