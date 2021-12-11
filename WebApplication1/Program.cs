using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using ClassLibrary1;
using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;
using ClassLibrary1.Infrastructure;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using System.Text.Json;

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

app.MapGet(Endpoints.BIOS, async (HttpContext httpContext) =>
{
    #region TODO

    // Refactor Sections to ClassLibrary1
    // LoggingBehaviors? (Identity, Performance, Color)
    // SeriLog? ApplicationInsights?
    // https://docs.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider    #endregion TODO

    #endregion TODO

    app.Logger.LogInformation("MapGet".TagWhere());

    app.Logger.LogInformation("PreAuthorizationLogic".TagWhy());

    httpContext.ValidateAppRole(AppRole.CanRead);

    app.Logger.LogInformation("ValidatedCanReadPermission".TagWhy());

    httpContext.ValidateAppRole(AppRole.CanWrite);

    app.Logger.LogInformation("ValidatedCanWritePermission".TagWhy());

    app.Logger.LogInformation("PostAuthorizationLogic".TagWhy());

    app.Logger.LogInformation("PreApplicationLogic".TagWhy());

    var id = Guid.NewGuid();

    var packet = new Packet()
    {
        Id = id,
        ReferenceId = id,
        Sent = false,
        Received = false,
        Color = PacketColor.Green
    };

    #region Local Storage Service

    bool LocalStorageServiceEnabled = true;

    if (LocalStorageServiceEnabled)
    {
        var service = new LocalStorageService();

        var result = await LocalStorageService.IO(packet);
    };

    #endregion Local Storage Service

    #region Azure Storage Service

    bool AzureStorageServiceEnabled = false;

    if (AzureStorageServiceEnabled)
    {
        string storageUri = "";
        string accountName = "";
        string storageAccountKey = "";

        // Construct a new "TableServiceClient using a TableSharedKeyCredential.
        var serviceClient = new TableServiceClient(
            new Uri(storageUri),
            new TableSharedKeyCredential(accountName, storageAccountKey));

        // Create a new table. The TableItem class stores properties of the created table.
        string tableName = "NewVeryImportantTable";
        //TableItem table = serviceClient.CreateTableIfNotExists(tableName);
        //Tag.What($"table.Name={table.Name}");

        // Construct a new <see cref="TableClient" /> using a <see cref="TableSharedKeyCredential" />.
        var tableClient = new TableClient(
            new Uri(storageUri),
            tableName,
            new TableSharedKeyCredential(accountName, storageAccountKey));

        // Create the table in the service.
        //tableClient.Create();

        Tag.When("");
        Tag.What(tableName);

        var packetTableEntity = new PacketTableEntity
        {
            Id = packet.Id.ToString(),
            ReferenceId = packet.ReferenceId.ToString(),
            Color = packet.Color.Code,
            PartitionKey = packet.Id.ToString(),
            RowKey = packet.Id.ToString(),
        };

        //tableClient.AddEntity(strongEntity);
        tableClient.AddEntity(packetTableEntity);

        Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{packet.Id}'");

        // Iterate the <see cref="Pageable"> to access all queried entities.
        foreach (TableEntity qEntity in queryResultsFilter)
        {
            Tag.What($"{qEntity.GetString("Id")}");
            Tag.What($"{qEntity.GetString("ReferenceId")}");
            Tag.What($"{qEntity.GetString("Color")}");

            var storedPacket = new Packet()
            {
                Id = Guid.Parse(qEntity.GetString("Id")),
                ReferenceId = Guid.Parse(qEntity.GetString("ReferenceId")),
                Color = new PacketColor(qEntity.GetString("Color"))
            };

            Tag.What($"storedPacket={storedPacket}");
        }

        Tag.What($"queryResultsFilter.Count={queryResultsFilter.Count()}");
    }

    #endregion Azure Storage Service

    app.Logger.LogInformation($"packet={packet}".TagWhat());

    app.Logger.LogInformation("PostApplicationLogic".TagWhy());

    //return packets;
    return packet;
})
.WithName("BasicInputOutputSystem")
.RequireAuthorization();

app.Run();

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program { }
#pragma warning restore CA1050 // Declare types in namespaces
