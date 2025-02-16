using FlashApp.IdentityHost.Constants;
using FlashApp.Infrastructure.Configuration;

namespace FlashApp.IdentityHost;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
       // AddIdentity(services, configuration);
        AddIdentityServer(services, configuration);
        return services;
    }

    // private static void AddIdentity(IServiceCollection services, IConfiguration configuration)
    // {
    //     var connectionString = configuration.GetConnectionString("DefaultConnection");
    //
    //     services.AddDbContext<FlashAppDbContext>(options =>
    //         options.UseSqlServer(connectionString));
    //
    //     services.AddIdentity<ApplicationUser, Role>()
    //         .AddEntityFrameworkStores<FlashAppDbContext>()
    //         .AddDefaultTokenProviders();
    // }

    private static void AddIdentityServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.Authentication.CookieLifetime = TimeSpan.FromDays(30);
            options.Authentication.CookieSlidingExpiration = true;

            options.UserInteraction.LoginUrl = IdentityDefaults.LoginUrl;
            options.UserInteraction.LogoutUrl = IdentityDefaults.LogoutUrl;
        })
        // .AddAspNetIdentity<ApplicationUser>()
        .AddInMemoryClients(IdentityDefaults.TestData.Clients)
        .AddInMemoryApiScopes(IdentityDefaults.TestData.ApiScopes)
        .AddInMemoryIdentityResources(IdentityDefaults.TestData.IdentityResources)
        .AddDeveloperSigningCredential();
    }
}
