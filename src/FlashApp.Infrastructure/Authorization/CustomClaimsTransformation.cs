using FlashApp.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace FlashApp.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation(IServiceProvider serviceProvider) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(claim => claim.Type == ClaimTypes.Role) &&
            principal.HasClaim(claim => claim.Type == JwtRegisteredClaimNames.Sub))
        {
            return principal;
        }

        using IServiceScope scope = serviceProvider.CreateScope();

        string identityId = principal.GetIdentityId();
        

        var claimsIdentity = new ClaimsIdentity();
        // claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, userRoles.Id.ToString()));
        //
        // foreach (var role in userRoles.Roles)
        // {
        //     claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
        // }

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
