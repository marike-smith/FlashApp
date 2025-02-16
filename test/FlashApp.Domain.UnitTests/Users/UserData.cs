using FlashApp.Domain.Entities.Users;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Domain.UnitTests.Users;
internal static class UserData
{
    public static User Create() => User.Create(FirstName, LastName, PhoneNumber, Email, Password);
    public const string FirstName = "First";
    public const string LastName = "Last";
    public static readonly Email Email = new("test@test.com");
    public static readonly Password Password = new("Password");
    public static readonly string PhoneNumber = "1234567890";
}
