using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Diagnostics;

const string authenticatedEndpoint = "/authenticatedEndpoint";
const string unauthenticatedEndpoint = "/unauthenticatedEndpoint";

Trace.TraceInformation("@Program.cs");

Trace.TraceInformation("@CreateBuilder()");
var builder = WebApplication.CreateBuilder(args);

Trace.TraceInformation("@AddAuthentication");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

Trace.TraceInformation("@AddAuthorization");
builder.Services.AddAuthorization();

Trace.TraceInformation("@AddEndpointsApiExplorer");
builder.Services.AddEndpointsApiExplorer();

Trace.TraceInformation("@AddSwaggerGen");
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Trace.TraceInformation("@Environment @IsDevelopment @True");

    Trace.TraceInformation("@UseSwagger");
    app.UseSwagger();

    Trace.TraceInformation("@UseSwaggerUI");
    app.UseSwaggerUI();
}

Trace.TraceInformation("@UseHttpsRedirection");
app.UseHttpsRedirection();

Trace.TraceInformation("@UseAuthentication");
app.UseAuthentication();

Trace.TraceInformation("@UseAuthorization");
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(unauthenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogInformation("@MapGet()");

    app.Logger.LogInformation("@UnauthenticatedEndpoint");
})
.WithName("UnauthenticatedEndpoint");

app.MapGet(authenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogInformation("@MapGet()");

    app.Logger.LogInformation("@AuthenticatedEndpoint");

    #region Audit Logic

    app.Logger.LogWarning("@TODO @RefactorAuditLogic");

    app.Logger.LogWarning("@TODO @AuditIdentity");

    app.Logger.LogWarning("@TODO @AuditClientIp");

    #endregion Audit Logic

    #region Authorization Logic

    app.Logger.LogInformation("@PreAuthorizationLogic @DaemonAppRole");

    app.Logger.LogWarning("@TODO @RefactorAuthorizationLogic");
    app.Logger.LogInformation("@AuthorizationLogic");
    httpContext.ValidateAppRole("DaemonAppRole");

    app.Logger.LogInformation("@PostAuthorizationLogic");

    app.Logger.LogInformation("@AuthorizedEndpoint");

    #endregion Authorization Logic

    #region Application Logic

    app.Logger.LogInformation("@PreApplicationLogic");

    app.Logger.LogWarning("@TODO @RefactorApplicationLogic");
    app.Logger.LogInformation("@ApplicationLogic");
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();

    app.Logger.LogInformation("@PostApplicationLogic");

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

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
