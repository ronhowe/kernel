using ClassLibrary1;
using Figgle;
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
    var photon = InMemoryStorageService.Read(id).Result;
    //var photon = LocalStorageService.Read(id).Result;
    //var photon = AzureTableStorageService.Read(id).Result;

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("GET")));
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render(photon.Color)));

    return photon;
})
.RequireAuthorization();

app.MapPost("/bios", (Photon photon, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("POST")));
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render(photon.Color)));

    //app.Logger.LogInformation(photon.ToString());

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

    var result = InMemoryStorageService.Write(photon);
    //var result = LocalStorageService.Write(photon);
    //var result = AzureTableStorageService.Write(photon);

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    return photon;
})
.RequireAuthorization();

app.Run();
