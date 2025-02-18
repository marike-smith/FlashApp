using FlashApp.Domain.Entities.SensitiveWord;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Application.UnitTests.Users;
internal static class UserData
{
    public static SensitiveWord Create() => SensitiveWord.Create(new Word( Word));

    private static readonly string Word = "First";

}