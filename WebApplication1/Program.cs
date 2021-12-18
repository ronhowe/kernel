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

    app.Logger.LogInformation($"id={id}".TagWhat());

    app.Logger.LogInformation("MakeAuthorizationConfigurable".TagToDo());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    httpContext.ValidateAppRole(ApplicationRole.CanRead);

    app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

    app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());

    app.Logger.LogInformation("Pre<IStorageService>Call".TagWhy());

    app.Logger.LogWarning("ImplementIStorageServiceInterface".TagToDo());
    app.Logger.LogWarning("ImplementAsyncServiceRead".TagToDo());
    var photon = NullStorageService.Read(id).Result;

    app.Logger.LogInformation("Post<IStorageService>Call".TagWhy());

    app.Logger.LogTrace("GET".TagShout());

    return photon;
})
.RequireAuthorization();

app.MapPost(ApplicationEndpoint.BasicInputOutputService, (Photon photon, HttpContext httpContext) =>
{
    app.Logger.LogInformation("MapPost".TagWhere());

    app.Logger.LogInformation($"photon={photon}".TagWhat());

    app.Logger.LogInformation("MakeAuthorizationConfigurable".TagToDo());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    httpContext.ValidateAppRole(ApplicationRole.CanRead);

    app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

    httpContext.ValidateAppRole(ApplicationRole.CanWrite);

    app.Logger.LogInformation("ValidatedCanWritePermission".TagWhy());

    app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());

    app.Logger.LogInformation("Pre<IStorageService>Call".TagWhy());

    app.Logger.LogWarning("ImplementIStorageServiceInterface".TagToDo());
    app.Logger.LogWarning("ImplementAsyncServiceWrite".TagToDo());
    var result = NullStorageService.Write(photon);

    app.Logger.LogInformation("Post<IStorageService>Call".TagWhy());

    app.Logger.LogTrace($"POST {photon.Color}".TagShout());

    return photon;
})
.RequireAuthorization();

app.Run();
