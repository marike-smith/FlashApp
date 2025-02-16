using FlashApp.Application.Abstractions.Messaging;

namespace FlashApp.Application.Users.GetLoggedInUser;
public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
