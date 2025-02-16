using Asp.Versioning;
using Dapper;
using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Caching;
using FlashApp.Application.Abstractions.Data;
using FlashApp.Application.Abstractions.Email;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;
using FlashApp.Domain.Entities.Roles;
using FlashApp.Infrastructure.Authentication;
using FlashApp.Infrastructure.Authorization;
using FlashApp.Infrastructure.Caching;
using FlashApp.Infrastructure.Configuration;
using FlashApp.Infrastructure.Data;
using FlashApp.Infrastructure.Email;
using FlashApp.Infrastructure.Interceptors;
using FlashApp.Infrastructure.Outbox;
using FlashApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using AuthenticationService = FlashApp.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = FlashApp.Application.Abstractions.Authentication.IAuthenticationService;
using IdentityOptions = FlashApp.Infrastructure.Configuration.IdentityOptions;

namespace FlashApp.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddAppConfigurations(services, configuration);

        AddPersistence(services, configuration);

        AddSwaggerWithSecurity(services, configuration);

        AddAuthentication(services, configuration);
        
        AddAuthorization(services);

        AddBackgroundJobs(services, configuration);

        AddCaching(services, configuration);

        AddHealthChecks(services, configuration);

        AddApiVersioning(services);
        

        return services;
    }

    private static void AddAppConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection(nameof(IdentityOptions)));
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
    }

    private static void AddBackgroundJobs(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));

        services.AddQuartz(configurator =>
        {
            var schedulerId = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{schedulerId}";
            configurator.SchedulerName = $"default-name-{schedulerId}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ProcessOutboxMessageJobSetup>();
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddDbContext<FlashAppDbContext>((sp, options) => options.UseSqlServer(connectionString)
            .AddInterceptors(
                sp.GetRequiredService<UpdateAuditableInterceptor>()));
        
        services.AddIdentity<ApplicationUser, Role>()
            .AddEntityFrameworkStores<FlashAppDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepository<User, int>, UserRepository>();
        services.AddScoped<IRepository<ApplicationUser, int>, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<FlashAppDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
    
     private static IServiceCollection AddSwaggerWithSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options => ConfigureGenSwaggerOptions(options, configuration));
        return services;
    }

    private static void ConfigureGenSwaggerOptions(SwaggerGenOptions options, IConfiguration configuration)
    {
        var identityConfiguration = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();
        options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(identityConfiguration.AuthorizationEndpoint),
                    TokenUrl = new Uri(identityConfiguration.TokenEndpoint),
                    Scopes = new Dictionary<string, string>
                    {
                        { "api.read", "Read access to API" },
                        { "api.write", "Write access to API" }
                    }
                }
            }
        });
        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "API Key required in the 'X-API-KEY' header",
            Type = SecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = ParameterLocation.Header
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" }
                },
                new List<string> { "api.read", "api.write" }
            },
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                },
                new List<string>()
            }
        });
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var authenticationOptions = configuration.Get<IdentityOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyAuth", null)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authenticationOptions.TokenEndpoint;
                options.Audience = authenticationOptions.Audience; 
            });

        services.ConfigureOptions<JwtBearerOptionsSetup>();


        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpclient) =>
        {
            httpclient.BaseAddress = new Uri(authenticationOptions.TokenEndpoint);
        });

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpclient) =>
        {
            httpclient.BaseAddress = new Uri(authenticationOptions.TokenEndpoint);
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }
    
    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddScoped<AuthorizationService>();

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Cache") ??
                                throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .AddRedis(configuration.GetConnectionString("Cache"));
    }

    ///<summary>
    /// Add API Versioning when using Controllers
    /// If using Minimal APIs, consider the Nuget Package "Asp.Versioning.Http"
    /// </summary>
    /// <param name="services"></param>
    private static void AddApiVersioning(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });
    }
}
