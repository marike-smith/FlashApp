using FlashApp.Domain.Entities.Roles;

namespace FlashApp.Infrastructure.Authorization;

public sealed class UserRolesResponse
{
    public int Id { get; init; }
    public List<Role> Roles { get; init; } = [];
}
