using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Application.Auth.LogInUser;
internal sealed class LogInUserCommandHandler(IAuthenticationService authenticationService)
    : ICommandHandler<LogInUserCommand, LogInUserCommandResponse>
{
    public async Task<Result<LogInUserCommandResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginAsync(new Email(request.Email), new Password(request.Password), cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<LogInUserCommandResponse>(UserErrors.InvalidCredentials);
        }

        return Result.Success(new LogInUserCommandResponse());
    }
}
