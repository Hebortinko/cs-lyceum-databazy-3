using DatabazyApiStarter;
using DatabazyApiStarter.Models;
using DatabazyApiStarter.Repositories;
using DatabazyApiStarter.Services;

if (!LoadEnv.LoadFromDefaultLocations())
{
    throw new InvalidOperationException(
        "Could not find .env file. Copy .env.example to .env and fill in your database settings."
    );
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/health", () => Results.Ok(new
{
    success = true,
    message = "API is running."
}));

app.MapPost("/api/auth/login", async (LoginRequest request, AuthService authService) =>
{
    var result = await authService.LoginAsync(request);
    return Results.Ok(result);
});

app.Run();
