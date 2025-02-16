using FlashApp.Api.Middleware;
using FlashApp.API.Middleware;
using FlashApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FlashApp.API.Extensions;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// Run EF Migrations 
    /// </summary>
    /// <param name="app"></param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using FlashAppDbContext dbContext = scope.ServiceProvider.GetRequiredService<FlashAppDbContext>();

        dbContext.Database.MigrateAsync();
    }

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();

    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
       => app.UseMiddleware<RequestContextLoggingMiddleware>();
    
    public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();
}

