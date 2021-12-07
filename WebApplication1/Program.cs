using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Diagnostics;

const string authenticatedEndpoint = "/authenticatedEndpoint";
const string unauthenticatedEndpoint = "/unauthenticatedEndpoint";

Trace.WriteLine("@Program.cs");

Trace.WriteLine("@CreateBuilder()");
var builder = WebApplication.CreateBuilder(args);

Trace.WriteLine("@AddAuthentication");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

Trace.WriteLine("@AddAuthorization");
builder.Services.AddAuthorization();

Trace.WriteLine("@AddEndpointsApiExplorer");
builder.Services.AddEndpointsApiExplorer();

Trace.WriteLine("@AddSwaggerGen");
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Trace.WriteLine("@IsDevelopmentEnvironment");

    Trace.WriteLine("@UseSwagger");
    app.UseSwagger();

    Trace.WriteLine("@UseSwaggerUI");
    app.UseSwaggerUI();
}

Trace.WriteLine("@UseHttpsRedirection");
app.UseHttpsRedirection();

Trace.WriteLine("@UseAuthentication");
app.UseAuthentication();

Trace.WriteLine("@UseAuthorization");
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(unauthenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");

    app.Logger.LogTrace("@UnauthenticatedEndpoint");
})
.WithName("UnauthenticatedEndpoint");

app.MapGet(authenticatedEndpoint, (HttpContext httpContext) =>
{
    app.Logger.LogTrace("@MapGet()");

    app.Logger.LogTrace("@AuthenticatedEndpoint");

    #region Audit Logic

    //app.Logger.LogWarning("@TODO @RefactorAuditLogic");

    //app.Logger.LogWarning("@TODO @AuditIdentity");

    #endregion Audit Logic

    #region Authorization Logic

    app.Logger.LogTrace("@PreAuthorizationLogic @DaemonAppRole");

    //app.Logger.LogWarning("@TODO @RefactorAuthorizationLogic");

    app.Logger.LogTrace("@AuthorizationLogic");

    foreach (var claim in httpContext.User.Claims)
    {
        app.Logger.LogTrace($"\n@claim.Type={claim.Type} \n@claim.Value={claim.Value}\n@claim.ValueType={claim.ValueType}\n@claim.Subject.Name={claim.Subject.Name}\n@claim.Issuer={claim.Issuer}\n");
    }

    httpContext.ValidateAppRole("DaemonAppRole");
    httpContext.ValidateAppRole("DataWriterRole");

    app.Logger.LogTrace("@PostAuthorizationLogic");

    app.Logger.LogTrace("@AuthorizedEndpoint");

    #endregion Authorization Logic

    #region Application Logic

    app.Logger.LogTrace("@PreApplicationLogic @ExternalDependency");

    //app.Logger.LogWarning("@TODO @RefactorApplicationLogic");
    app.Logger.LogTrace("@ApplicationLogic");
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();

    app.Logger.LogTrace("@PostApplicationLogic @ExternalDependency");

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
