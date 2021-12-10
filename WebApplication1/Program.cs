using Azure;
using Azure.Data.Tables;
using ClassLibrary1;
using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
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

app.MapGet(Endpoints.BIOS, (HttpContext httpContext) =>
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

    #region Hard-Coded

    var packet = new Packet
    {
        Id = Guid.NewGuid(),
        ReferenceId = Guid.NewGuid(),
        Color = ClassLibrary1.Domain.ValueObjects.PacketColor.Green
    };

    string jsonString = JsonSerializer.Serialize(packet);
    
    //var packetWriter = new WritePacketService();

    //Packet packet = new Packet();

    //packetWriter.Write(packet);

    //var packetReader = new ReadPacketService();

    //var newPacket = packetReader.Read(packet.Id);

    //Assert.AreEqual(packet.Id, newPacket.Id);

    //var packets = Enumerable.Range(1, 1).Select(index => packetReader.Read(packet.Id)).ToArray();

    #endregion Hard-Coded

    #region Storage Account
    bool storageAccountIo = false;
    if (storageAccountIo)
    {
        string storageUri = "https://{0}.table.core.windows.net";
        string accountName = "YOUR_ACCOUNT_HERE";
        string storageAccountKey = "YOUR_ACCOUNT_HERE";

        // Construct a new "TableServiceClient using a TableSharedKeyCredential.
        var serviceClient = new TableServiceClient(
            new Uri(storageUri),
            new TableSharedKeyCredential(accountName, storageAccountKey));

        // Create a new table. The TableItem class stores properties of the created table.
        string tableName = "VeryImportantTable";
        //    TableItem table = serviceClient.CreateTableIfNotExists(tableName);
        //Tag.What($"table.Name={table.Name}");

        // Construct a new <see cref="TableClient" /> using a <see cref="TableSharedKeyCredential" />.
        var tableClient = new TableClient(
            new Uri(storageUri),
            tableName,
            new TableSharedKeyCredential(accountName, storageAccountKey));

        // Create the table in the service.
        tableClient.Create();

        Tag.When("");
        Tag.What(tableName);

        string partitionKey = Guid.NewGuid().ToString();
        string rowKeyStrong = Guid.NewGuid().ToString();

        // Make a dictionary entity by defining a <see cref="TableEntity">.
        // Create an instance of the strongly-typed entity and set their properties.
        var strongEntity = new OfficeSupplyEntity
        {
            PartitionKey = partitionKey,
            RowKey = rowKeyStrong,
            Product = "Notebook",
            Price = 3.00,
            Quantity = 50
        };

        tableClient.AddEntity(strongEntity);

        Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");

        // Iterate the <see cref="Pageable"> to access all queried entities.
        foreach (TableEntity qEntity in queryResultsFilter)
        {
            Tag.What($"{qEntity.GetString("Product")}: {qEntity.GetDouble("Price")}");
        }

        Tag.What($"queryResultsFilter.Count={queryResultsFilter.Count()}");
    }
    #endregion Storage Account

    //app.Logger.LogInformation($"packets.Length={packets.Length}".TagWhat());

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
