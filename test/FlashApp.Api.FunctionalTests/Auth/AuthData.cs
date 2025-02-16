using FlashApp.API.Controllers.Users;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Api.FunctionalTests.Auth;
public static class AuthData
{
    public static string UserLoginEndpoint = "api/v1/auth/login";
    public static RegisterUserRequest RegisterUserRequest = new("User@test.com", "test", "test", new Email("User@test.com"), new Password("testPass123!@#"));
}
