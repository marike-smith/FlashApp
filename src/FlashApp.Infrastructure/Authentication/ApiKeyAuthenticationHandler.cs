using System.Security.Claims;
using System.Text.Encodings.Web;
using FlashApp.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlashApp.Infrastructure.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IOptions<IdentityOptions> configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key was not provided"));
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            var configuredApiKey = configuration.Value.ApiKey;

            if (configuredApiKey == null || providedApiKey != configuredApiKey)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
            }

            // Authenticated successfully
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "ApiKeyUser"), // Example; modify as needed
                new Claim(ClaimTypes.Name, "ApiKeyAuthentication")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}