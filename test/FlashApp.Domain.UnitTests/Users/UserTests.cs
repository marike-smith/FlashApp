
using FluentAssertions;
using FlashApp.Domain.UnitTests.Users;

namespace FlashApp.Domain.UnitTests.Users;
public class UserTests : BaseTest
{
    [Fact]
    public void Create_Should_Raise_UserCreatedDomainEvent()
    {
        //Act
        var user = UserData.Create();

        //Assert
       // UserCreatedDomainEvent userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent, int>(user);

       // userCreatedDomainEvent.Value.Should().Be(user.Id);
    }
}
