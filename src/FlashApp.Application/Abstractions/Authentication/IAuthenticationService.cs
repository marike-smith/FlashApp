using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Application.Abstractions.Authentication;
public interface IAuthenticationService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The identifier of the newly registered user</returns>
    Task<Result> RegisterAsync(
        ApplicationUser user,
        Password password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// User Login
    /// </summary>
    /// <param name="applicationUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result> LoginAsync(Domain.Shared.ValueObjects.Email email, Password password, CancellationToken cancellationToken = default);
}
