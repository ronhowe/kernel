using ClassLibrary1.Common;
using ClassLibrary1.Infrastructure;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

Tag.How("Program");

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

app.MapGet(Endpoints.POST, (HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());
})
.WithName("PowerOnSelfTest");

app.MapGet(Endpoints.BIOS, (HttpContext httpContext) =>
{
    #region TODO
    // Refactor Sections to ClassLibrary1
    // LoggingBehaviors? (Identity, Performance, Color)
    // SeriLog? ApplicationInsights?
    #endregion TODO

    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    #region Authorization Logic

    app.Logger.LogInformation("AuthorizationLogic".TagWhy());

    httpContext.ValidateAppRole(AppRole.CanRead);

    httpContext.ValidateAppRole(AppRole.CanWrite);

    #endregion Authorization Logic

    app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());

    app.Logger.LogInformation("PreApplicationLogic".TagWhy());

    #region Application Logic

    var service = new ReadPacketService();

    var packets = Enumerable.Range(1, 1).Select(index => service.Read()).ToArray();

    app.Logger.LogInformation($"packets.Length={packets.Length}".TagWhat());

    #endregion Application Logic

    app.Logger.LogInformation("PostApplicationLogic".TagWhy());


    return packets;
})
.WithName("BasicInputOutputSystem")
.RequireAuthorization();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
