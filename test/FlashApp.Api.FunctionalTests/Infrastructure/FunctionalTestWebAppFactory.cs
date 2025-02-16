using FlashApp.Application.Abstractions.Data;
using FlashApp.Infrastructure;
using FlashApp.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.Redis;
using Testcontainers.MsSql;
using FlashApp.Infrastructure.Configuration;


namespace FlashApp.Api.FunctionalTests.Infrastructure;
public class FunctionalTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
      .WithImage("mcr.microsoft.com/mssql/server:latest")
      .Build();


    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureTestServices(services =>
        {
            #region Wiring up any dependencies on the database container

            services.RemoveAll(typeof(DbContextOptions<FlashAppDbContext>));

            services.AddDbContext<FlashAppDbContext>(options =>
                options
                .UseSqlServer(_dbContainer.GetConnectionString())
                .UseSnakeCaseNamingConvention());

            services.RemoveAll(typeof(ISqlConnectionFactory));

            services.AddSingleton<ISqlConnectionFactory>(_ =>
                new SqlConnectionFactory(_dbContainer.GetConnectionString()));

            #endregion

            services.Configure<RedisCacheOptions>(redisCacheOptions =>
                redisCacheOptions.Configuration = _redisContainer.GetConnectionString());


            services.Configure<IdentityOptions>(o =>
            {

            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();

        await InitializeTestUserAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _redisContainer.StopAsync();
    }

    private async Task InitializeTestUserAsync()
    {
        HttpClient httpClient = CreateClient();
        //
        // await httpClient.PostAsJsonAsync(SellerData.RegistrationEndpoint, AuthData.RegisterSellerRequest);
    }
}
