using FlashApp.Application.Abstractions.Messaging;

namespace FlashApp.Application.Auth.LogInUser;
public sealed record LogInUserCommand(string Email, string Password) : ICommand<LogInUserCommandResponse>;
