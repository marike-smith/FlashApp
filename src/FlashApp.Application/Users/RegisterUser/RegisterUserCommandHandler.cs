using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;
using Microsoft.Extensions.Logging;

namespace FlashApp.Application.Users.RegisterUser;
internal sealed class RegisterUserCommandHandler(IAuthenticationService authenticationService, IUserRepository UserRepository, IUnitOfWork unitOfWork, ILogger<RegisterUserCommandHandler> logger) : ICommandHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    public async Task<Result<RegisterUserCommandResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var newUser = User.Create(
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.EmailAddress,
                request.Password);

            var result = await authenticationService.RegisterAsync(
                newUser,
                request.Password,
                cancellationToken);

            if (result.IsFailure) return Result.Failure<RegisterUserCommandResponse>(UserErrors.RegistrationFailure);

            newUser.SetIdentityId(newUser.Id);

            await UserRepository.AddAsync(newUser, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new RegisterUserCommandResponse(newUser.IdentityId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, nameof(RegisterUserCommand));
            return Result.Failure<RegisterUserCommandResponse>(UserErrors.RegistrationFailure);
        }
    }
}
