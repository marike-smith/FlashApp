using FlashApp.Application.Abstractions.Messaging;

namespace FlashApp.Application.Auth.GetLoggedInUser;
public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;
