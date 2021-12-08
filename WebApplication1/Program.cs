using ClassLibrary1.Infrastructure;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Diagnostics;

Trace.WriteLine("@Program.cs");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(Endpoints.POST, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");
})
.WithName("PowerOnSelfTest");

app.MapGet(Endpoints.BIOS, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");

    #region Audit Logic

    // @TODO @RefactorAuditLogic

    // @TODO @AuditIdentity

    #endregion Audit Logic

    #region Authorization Logic

    app.Logger.LogTrace("@PreAuthorizationLogic");

    // @TODO @RefactorAuthorizationLogic

    app.Logger.LogTrace("@AuthorizationLogic");

    httpContext.ValidateAppRole(AppRole.CanRead);

    httpContext.ValidateAppRole(AppRole.CanWrite);

    app.Logger.LogTrace("@PostAuthorizationLogic");

    #endregion Authorization Logic

    #region Application Logic

    app.Logger.LogTrace("@PreApplicationLogic");

    // @TODO @RefactorApplicationLogic

    app.Logger.LogTrace("@ApplicationLogic");

    var forecast = Enumerable.Range(1, 1).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();

    app.Logger.LogTrace("@PostApplicationLogic");

    return forecast;

    #endregion Application Logic
})
.WithName("BasicInputOutputSystem")
.RequireAuthorization();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
