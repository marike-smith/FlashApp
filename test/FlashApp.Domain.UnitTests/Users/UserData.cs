using FlashApp.Domain.Entities.SensitiveWord;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Domain.UnitTests.Users;
internal static class UserData
{
    public static SensitiveWord Create() => SensitiveWord.Create(new Word( Word));
    private const string Word = "SELECT";
}
