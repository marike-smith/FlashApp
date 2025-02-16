using FluentAssertions;
using FlashApp.Api.FunctionalTests.Auth;
using FlashApp.Api.FunctionalTests.Infrastructure;
using FlashApp.API.Controllers.Users;
using FlashApp.Domain.Shared.ValueObjects;
using System.Net;
using System.Net.Http.Json;

namespace FlashApp.Api.FunctionalTests.Users;
public class RegisterUserTests : BaseFunctionalTest
{
    public RegisterUserTests(FunctionalTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact(Skip = "TODO: Resolve failing test.")]
    public async Task Register_ShouldReturnOk_WhenRequestIsValid()
    {
        //Arrange
        var request = AuthData.RegisterUserRequest;

        //Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(UserData.RegistrationEndpoint, request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Theory(Skip = "TODO: Resolve failing test.")]
    [InlineData("", "first", "last", "12345", "gh@839023")]
    [InlineData("test.com", "first", "last", "12345", "g!@#023")]
    [InlineData("@test.com", "first", "last", "12345", "gh@8390345")]
    [InlineData("test@", "first", "last", "12345", "gh@839023")]
    [InlineData("test@test.com", "", "last", "12345", "123#$%ghj")]
    [InlineData("test@test.com", "first", "", "12345", "#$%@839027")]
    [InlineData("test@test.com", "first", "last", "543756783", "!@#RTYghj")]
    [InlineData("test@test.com", "first", "last", "1", "hhjKKH$%!")]
    [InlineData("test@test.com", "first", "last", "12", "rTH$%!")]
    [InlineData("test@test.com", "first", "last", "123", "uioTH$%!")]
    [InlineData("test@test.com", "first", "last", "1234", "dfgTH$%!")]
    public async Task Register_ShouldReturnBadRequest_WhenRequestIsInvalid(
        string email,
        string firstName,
        string lastName,
        string phoneNumber,
        string password)
    {
        //Arrange
        var request = new RegisterUserRequest(firstName, lastName, phoneNumber, new Email(email), new Password(password));

        //Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(UserData.RegistrationEndpoint, request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
