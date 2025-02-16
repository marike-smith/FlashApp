using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace FlashApp.Infrastructure.Authentication;
internal static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        string userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(userId, out int parsedUserId)
            ? parsedUserId :
            throw new ApplicationException("User identifier is unavailable");
    }

    public static string GetIdentityId(this ClaimsPrincipal principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ??
               throw new ApplicationException("User identity is unavailable");
    }
}
