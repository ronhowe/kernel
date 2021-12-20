using ClassLibrary1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.FeatureManagement;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Serilog;

Log.Logger = new LoggerConfiguration()
    //.WriteTo.Console()
    .WriteTo.Trace()
    //.CreateBootstrapLogger();
    .CreateLogger();

try
{
    Log.Information("Starting up");

    Tag.How("Program");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

    builder.Services.AddAuthorization();

    builder.Services.AddHealthChecks();

    builder.Services.AddFeatureManagement();

    builder.Services.AddAzureAppConfiguration();

    var connectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");

    Tag.Why("PreAddAzureAppConfiguration");

    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options
            .Connect(connectionString)
            //.Connect(new Uri(settings["AppConfig:Endpoint"]), new ManagedIdentityCredential())
            //.Connect(new Uri(settings["AppConfig:Endpoint"]), new DefaultAzureCredential(true))
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("sentinel", refreshAll: true)
                .SetCacheExpiration(new TimeSpan(0, 0, 3));
            })
            .UseFeatureFlags(featureFlagOptions =>
            {
                featureFlagOptions.CacheExpirationInterval = new TimeSpan(0, 0, 3);
            });
    });

    Tag.Why("PostAddAzureAppConfiguration");

    var app = builder.Build();

    app.UseHttpsRedirection();

    app.UseAzureAppConfiguration();

    app.UseHealthChecks(ApplicationEndpoint.PowerOnSelfTest);

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

        if (app.Configuration.GetValue<string>("this") == "that")
        {
            throw new ApplicationException("MOCK DEATH BY CONFIGURATION");
        }

        if (app.Services.GetRequiredService<IFeatureManager>().IsEnabledAsync(nameof(FeatureFlags.MockApplicationException)).Result)
        {
            throw new ApplicationException("MOCK DEATH BY FEATURE");
        }

        app.Logger.LogInformation(app.Configuration.GetValue<string>("this"));

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}