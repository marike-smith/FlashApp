using FlashApp.Api.FunctionalTests.Auth;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace FlashApp.Api.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient HttpClient = factory.CreateClient();

    protected async Task<string> GetAccessToken()
    {
        // HttpResponseMessage loginResponse = await HttpClient.PostAsJsonAsync(
        //     AuthData.UserLoginEndpoint,
        //     // new LoginUserRequest(
        //     //     AuthData.RegisterUserRequest.EmailAddress.Value,
        //     //     AuthData.RegisterUserRequest.Password.Value));
        //
        //     // AccessTokenResponse? accessTokenResponse = await loginResponse.Content.ReadFromJsonAsync<AccessTokenResponse>();
        //     //
        //     // return accessTokenResponse!.AccessToken;
        // );
        throw new NotImplementedException();
    }
}
