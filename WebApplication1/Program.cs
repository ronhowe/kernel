using ClassLibrary1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

Tag.How("Program");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet(ApplicationEndpoint.BasicInputOutputService, (Guid id, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    Tag.ToDo("@MakeAuthorizationConfigurable");
    if (true)
    {
        httpContext.ValidateAppRole(ApplicationRole.CanRead);

        app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

        app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());
    }

    app.Logger.LogInformation("PreLocalStorageServiceCall".TagWhy());

    app.Logger.LogWarning("IsThisAsynchronous".TagToDo());
    var photon = VirtualStorageService.Read(id).Result;
    //var photon = LocalStorageService.Read(id).Result;
    //var photon = AzureTableStorageService.Read(id).Result;

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    app.Logger.LogInformation(Tag.Shout($"GET {photon.Color}"));

    return photon;
})
.RequireAuthorization();

app.MapPost(ApplicationEndpoint.BasicInputOutputService, (Photon photon, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation(Tag.Shout($"POST {photon.Color}"));

    Tag.ToDo("@MakeAuthorizationConfigurable");
    if (false)
    {
#pragma warning disable CS0162 // Unreachable code detected
        app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());
#pragma warning restore CS0162 // Unreachable code detected

        httpContext.ValidateAppRole(ApplicationRole.CanRead);

        app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

        httpContext.ValidateAppRole(ApplicationRole.CanWrite);

        app.Logger.LogInformation("ValidatedCanWritePermission".TagWhy());

        app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());
    }

    app.Logger.LogInformation("PreLocalStorageServiceCall".TagWhy());

    var result = VirtualStorageService.Write(photon);
    //var result = LocalStorageService.Write(photon);
    //var result = AzureTableStorageService.Write(photon);

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    return photon;
})
.RequireAuthorization();

app.Run();
