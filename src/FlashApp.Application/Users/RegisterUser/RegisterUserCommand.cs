using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Application.Users.RegisterUser;
public sealed record RegisterUserCommand(
   string FirstName, string LastName, string PhoneNumber, Email EmailAddress, Password Password) : ICommand<RegisterUserCommandResponse>;
