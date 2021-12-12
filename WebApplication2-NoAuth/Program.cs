using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Services;
using Figgle;
using Microsoft.Identity.Web.Resource;

Tag.How("Program");

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/bios", (Guid id, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    Tag.ToDo("@MakeAuthorizationConfigurable");
    if (false)
    {
        httpContext.ValidateAppRole(AppRole.CanRead);

        app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

        app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());
    }

    app.Logger.LogInformation("PreLocalStorageServiceCall".TagWhy());

    app.Logger.LogWarning("IsThisAsynchronous".TagToDo());
    var packet = LocalStorageService.Read(id).Result;

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("GET")));
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render(packet.Color)));

    return packet;
});

app.MapPost("/bios", (Packet packet, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render("POST")));
    app.Logger.LogInformation(Tag.Line(FiggleFonts.Standard.Render(packet.Color)));

    //app.Logger.LogInformation(packet.ToString());

    Tag.ToDo("@MakeAuthorizationConfigurable");
    if (false)
    {
        app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

        httpContext.ValidateAppRole(AppRole.CanRead);

        app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

        httpContext.ValidateAppRole(AppRole.CanWrite);

        app.Logger.LogInformation("ValidatedCanWritePermission".TagWhy());

        app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());
    }

    app.Logger.LogInformation("PreLocalStorageServiceCall".TagWhy());

    var result = LocalStorageService.Write(packet);

    app.Logger.LogInformation("PostLocalStorageServiceCall".TagWhy());

    return packet;
});

app.Run();
