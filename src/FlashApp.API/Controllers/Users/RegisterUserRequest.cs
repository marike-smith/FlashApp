using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.API.Controllers.Users;

public sealed record RegisterUserRequest(
    string FirstName, string LastName, string PhoneNumber, Email EmailAddress, Password Password);
