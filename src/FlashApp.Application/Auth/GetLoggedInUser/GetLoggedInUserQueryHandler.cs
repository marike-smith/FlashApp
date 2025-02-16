using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;

namespace FlashApp.Application.Users.GetLoggedInUser;
internal sealed class GetLoggedInUserQueryHandler : IQueryHandler<GetLoggedInUserQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetLoggedInUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        Result<ApplicationUser> result = await _userRepository.GetByIdAsync((_userContext.Id), cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<UserResponse>(UserErrors.InvalidCredentials);
        }

        return new UserResponse { Id = result.Value.Id, Email = result.Value.Email, FirstName = result.Value.FirstName, LastName = result.Value.LastName };
    }
}
