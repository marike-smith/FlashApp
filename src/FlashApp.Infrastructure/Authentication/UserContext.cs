using FlashApp.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace FlashApp.Infrastructure.Authentication;
internal sealed class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public int Id =>
        _contextAccessor
            .HttpContext?.User
            .GetUserId() ?? throw new ApplicationException("User context is unavailable");

    public string IdentityId =>
        _contextAccessor
            .HttpContext?.User
            .GetIdentityId() ?? throw new ApplicationException("User context is unavailable");    
}
