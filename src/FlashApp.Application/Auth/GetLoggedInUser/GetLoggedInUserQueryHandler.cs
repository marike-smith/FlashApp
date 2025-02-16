using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;

namespace FlashApp.Application.Auth.GetLoggedInUser;
internal sealed class GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
    : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        Result<ApplicationUser> result = await userRepository.GetByIdAsync((userContext.Id), cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<UserResponse>(UserErrors.InvalidCredentials);
        }

        return new UserResponse { Id = result.Value.Id, Email = result.Value.Email, FirstName = result.Value.FirstName, LastName = result.Value.LastName };
    }
}
