using Microsoft.AspNetCore.Authorization;

namespace FlashApp.Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        :base(permission)
    {        
    }
}
