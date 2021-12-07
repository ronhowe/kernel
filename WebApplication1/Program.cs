using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Diagnostics;

const string authenticatedEndpoint = "/authenticatedEndpoint";
const string unauthenticatedEndpoint = "/unauthenticatedEndpoint";

Trace.WriteLine("@Program.cs");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(unauthenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");
})
.WithName("UnauthenticatedEndpoint");

app.MapGet(authenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");

    #region Audit Logic

    // @TODO @RefactorAuditLogic

    // @TODO @AuditIdentity

    #endregion Audit Logic

    #region Authorization Logic

    app.Logger.LogTrace("@PreAuthorizationLogic");

    // @TODO @RefactorAuthorizationLogic

    //foreach (var claim in httpContext.User.Claims)
    //{
    //    app.Logger.LogTrace($"\n@claim.Type={claim.Type} \n@claim.Value={claim.Value}\n@claim.ValueType={claim.ValueType}\n@claim.Subject.Name={claim.Subject.Name}\n@claim.Issuer={claim.Issuer}\n");
    //}

    // e.g. Can Read
    httpContext.ValidateAppRole("DaemonAppRole");

    // e.g. Can Write
    httpContext.ValidateAppRole("DataWriterRole");

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
.WithName("AuthenticatedEndpoint")
.RequireAuthorization();

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
