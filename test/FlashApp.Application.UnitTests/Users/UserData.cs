using FlashApp.Domain.Entities.Users;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Application.UnitTests.Users;
internal static class UserData
{
    public static User Create() => User.Create(FirstName, LastName, PhoneNumber, Email, Password);

    public static readonly int Id = 42;
    public static readonly string FirstName = "First";
    public static readonly string LastName = "Last";
    public static readonly string PhoneNumber = "01234567891";
    public static readonly Email Email = new("test@test.com");
    public static readonly Password Password = new("Password123!@#");

    public static readonly string CompanyName = "TestCompany";
    public static readonly string TradingName = "HighlyOriginalName";
    public static readonly bool IsSoleProprietor = true;
    public static readonly string VatNumber = "Company";
}