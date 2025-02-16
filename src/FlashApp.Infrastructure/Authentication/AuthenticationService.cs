using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;
using FlashApp.Domain.Shared.ValueObjects;
using FlashApp.Infrastructure.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FlashApp.Infrastructure.Authentication;
internal sealed class AuthenticationService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IPasswordHasher<ApplicationUser> passwordHasher,
    CustomClaimsTransformation claimsTransformation,
    ILogger<AuthenticationService> logger)
    : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    private readonly CustomClaimsTransformation _claimsTransformation = claimsTransformation ?? throw new ArgumentNullException(nameof(claimsTransformation));
    private readonly ILogger<AuthenticationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> LoginAsync(Domain.Shared.ValueObjects.Email email, Password password, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email.Value);
            if (user is null)
            {
                _logger.LogWarning("{Error}", UserErrors.NotFound);
                return Result.Failure(UserErrors.NotFound);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password.Value, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("{Email}", UserErrors.InvalidCredentials);
                return Result.Failure(UserErrors.InvalidCredentials);
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            await _claimsTransformation.TransformAsync(principal);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Error}", UserErrors.LoginFailure);
            return Result.Failure(UserErrors.LoginFailure);
        }
    }

    public async Task<Result> RegisterAsync(ApplicationUser applicationUser, Password password, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _userManager.FindByEmailAsync(applicationUser.Email) is not null)
            {
                return Result.Failure(UserErrors.AlreadyExists);
            }

            applicationUser.PasswordHash = _passwordHasher.HashPassword(applicationUser, password.Value);
            var result = await _userManager.CreateAsync(applicationUser, password.Value);

            return result.Succeeded 
                ? Result.Success() 
                : Result.Failure(UserErrors.RegistrationFailure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Error}", UserErrors.RegistrationFailure);
            return Result.Failure(UserErrors.RegistrationFailure);
        }
    }
}