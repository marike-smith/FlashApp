using FlashApp.Application.Abstractions.Caching;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Roles;
using Microsoft.AspNetCore.Identity;

namespace FlashApp.Infrastructure.Authorization;

internal sealed class AuthorizationService(
    UserManager<ApplicationUser> userManager,
    ICacheService cacheService)
{
    public async Task<UserRolesResponse> GetRolesForUserAsync(string identityId)
    {
        string cacheKey = $"auth:roles-{identityId}";

        UserRolesResponse cachedRoles = await cacheService.GetAsync<UserRolesResponse>(cacheKey);
        if (cachedRoles is not null)
        {
            return cachedRoles;
        }

        var user = await userManager.FindByIdAsync(identityId);
        if (user is null)
        {
            return null; 
        }

        var roles = await userManager.GetRolesAsync(user);
        var userRolesResponse = new UserRolesResponse
        {
            Id = user.Id,
            Roles = roles.Select(x => new Role(x)).ToList()
        };

        await cacheService.SetAsync(cacheKey, userRolesResponse);

        return userRolesResponse;
    }
}
