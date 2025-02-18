using HealthChecks.UI.Client;
using FlashApp.API.Extensions;
using FlashApp.API.OpenApi;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using FlashApp.Application;
using Serilog;
using FlashApp.Infrastructure;
using FlashApp.Infrastructure.Configuration;
using FlashApp.API.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Host.UseSerilog((context, configuration) =>
{
    var setting = context.Configuration.GetSection(nameof(LoggingOptions)).Get<LoggingOptions>();
    configuration
        .ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<ReservedKeywordSeederService>(); 

//in order to configure swagger with different versions 
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<Asp.Versioning.ApiExplorer.ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (string groupName in descriptions.Select(description => description.GroupName))
        {
            string url = $"/swagger/{groupName}/swagger.json";
            string name = groupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseSecurityHeadersMiddleware();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program;
