using FlashApp.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FlashApp.Application.IntegrationTests.Infrastructure;
public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope; // Resolve any scoped services.

    protected readonly ISender Sender; // To Send commands and queries to run the integration tests

    protected BaseIntegrationTest(IntegrationTestWebAppFactory webAppFactory)
    {
        _serviceScope = webAppFactory.Services.CreateScope();

        Sender = _serviceScope.ServiceProvider.GetRequiredService<ISender>();
    }
}
