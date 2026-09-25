using FlightDataDecoder.Core.Decoding;
using FlightDataDecoder.Core.Layout;

var builder = WebApplication.CreateBuilder(args);

// Le layout est chargé une seule fois au démarrage (chemin défini dans appsettings.json).
var layoutPath = builder.Configuration["LayoutPath"]
                 ?? throw new InvalidOperationException("LayoutPath manquant dans appsettings.json");
builder.Services.AddSingleton(LayoutLoader.FromFile(layoutPath));

// Autorise le futur front React (Vite tourne par défaut sur le port 5173).
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors();

app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/layout", (FrameLayout layout) => Results.Ok(layout));

app.MapPost("/api/decode", async (IFormFile file, FrameLayout layout) =>
{
    using var stream = new MemoryStream();
    await file.CopyToAsync(stream);

    try
    {
        return Results.Ok(FlightDecoder.Decode(stream.ToArray(), layout));
    }
    catch (ArgumentException exception)
    {
        return Results.BadRequest(new { error = exception.Message });
    }
})
.DisableAntiforgery(); // API sans formulaire HTML ni cookie : pas de jeton anti-CSRF nécessaire.

app.Run();
