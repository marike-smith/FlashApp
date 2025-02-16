using FlashApp.Api.FunctionalTests.Auth;
using FlashApp.API.Controllers.Auth;
using System.Net.Http.Json;
using FlashApp.Application.Auth.LogInUser;

namespace FlashApp.Api.FunctionalTests.Infrastructure;
public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient;

    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        HttpClient = factory.CreateClient();
    }

    protected async Task<string> GetAccessToken()
    {
        HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
            AuthData.UserLoginEndpoint,
            new LoginUserRequest(
                AuthData.RegisterUserRequest.EmailAddress.Value,
                AuthData.RegisterUserRequest.Password.Value));

        AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();

        return accessTokenResponse!.AccessToken;
    }
}
