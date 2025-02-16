using Microsoft.AspNetCore.Identity;

namespace FlashApp.Domain.Entities.Roles
{
    public class Role : IdentityRole<int>
    {
        public Role() : base()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }
    }
}
