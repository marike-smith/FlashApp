using Asp.Versioning;
using Dapper;
using FlashApp.Application.Abstractions.Authentication;
using FlashApp.Application.Abstractions.Caching;
using FlashApp.Application.Abstractions.Data;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.SensitiveWord;
using FlashApp.Infrastructure.Authentication;
using FlashApp.Infrastructure.Authorization;
using FlashApp.Infrastructure.Caching;
using FlashApp.Infrastructure.Configuration;
using FlashApp.Infrastructure.Data;
using FlashApp.Infrastructure.Outbox;
using FlashApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerGen;
using IdentityOptions = FlashApp.Infrastructure.Configuration.IdentityOptions;
using FlashApp.Infrastructure.Constants;

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
        
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
        
        services.AddScoped<ISensitiveWordRepository, SensitiveKeywordRepository>();
        
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

        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "Enter API Key in the 'X-API-KEY' header",
            Type = SecuritySchemeType.ApiKey,
            Name = "X-API-KEY",
            In = ParameterLocation.Header
        });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter Bearer Token",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
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
        var identityConfiguration = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>();

        services
            .AddAuthentication(options =>
            {
                // Allow Authentication via Bearer or Api Key
                options.DefaultAuthenticateScheme = "MultipleAuthSchemes";
                options.DefaultChallengeScheme = "MultipleAuthSchemes";
            })
            .AddPolicyScheme("MultipleAuthSchemes", "API Key or JWT", options =>
            {
                options.ForwardDefaultSelector = context =>
                
                {
                    var hasApiKey = context.Request.Headers.ContainsKey("X-API-KEY");
                    return hasApiKey ? CustomSecuritySchemes.ApiKey : JwtBearerDefaults.AuthenticationScheme;
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(CustomSecuritySchemes.ApiKey, null)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = identityConfiguration.TokenEndpoint;
                options.Audience = identityConfiguration.Audience;
            });

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddHttpClient<IJwtService, JwtService>((serviceProvider, httpclient) =>
        {
            httpclient.BaseAddress = new Uri(identityConfiguration.TokenEndpoint);
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
    }

    
    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionString("Cache") ??
                                throw new ArgumentNullException(nameof(configuration));

        services.AddStackExchangeRedisCache(options => options.Configuration = redisConnectionString);

        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void AddHealthChecks(IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                                  throw new ArgumentNullException(nameof(configuration));
        
        var redisConnectionString = configuration.GetConnectionString("Cache") ?? 
                                  throw new ArgumentNullException(nameof(configuration));
        services.AddHealthChecks()
            .AddSqlServer(sqlConnectionString)
            .AddRedis(redisConnectionString);
    }

    ///<summary>
    /// Add API Versioning when using Controllers
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
