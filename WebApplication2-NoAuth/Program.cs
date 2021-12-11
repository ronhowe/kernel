using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using Figgle;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (Guid id) =>
{
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("GET")));

    Packet packet = new Packet() { Id = id };

    return packet;
})
.WithName("GetWeatherForecast");

app.MapPost("/weatherforecast", (Packet packet) =>
{
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("POST")));

    app.Logger.LogInformation(packet.ToString());

    return packet;
})
.WithName("PostWeatherForecast");

app.Run();
