using FlashApp.IdentityHost;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseIdentityServer();

app.MapGet("/", (IHttpContextAccessor accessor
    ) => $"Now listening on port {accessor.HttpContext?.Connection.LocalPort}");

app.Run();