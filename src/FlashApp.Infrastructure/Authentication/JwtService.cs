using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Infrastructure.Authentication.Models;
using FlashApp.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace FlashApp.Infrastructure.Authentication;
internal sealed class JwtService : IJwtService
{
    private static readonly Error AuthenticationFailed = new("AuthenticationFailed",
        "Failed to acquire access token.");

    private readonly HttpClient _httpClient;
    private readonly IdentityOptions _authenticationOptions;

    public JwtService(HttpClient httpClient, IOptions<IdentityOptions> authenticationOptions)
    {
        _httpClient = httpClient;
        _authenticationOptions = authenticationOptions.Value;
    }

    public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _authenticationOptions.ClientId),
                new("client_secret", _authenticationOptions.ClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            using var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            HttpResponseMessage response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            AuthorizationToken authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);
            if (authorizationToken is null)
            {
                return Result.Failure<string>(AuthenticationFailed);
            }

            return authorizationToken.AccessToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }

    }
}
