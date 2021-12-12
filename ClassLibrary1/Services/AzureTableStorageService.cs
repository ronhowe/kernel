using Azure;
using Azure.Data.Tables;
using ClassLibrary1;
using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using ClassLibrary1.Domain.ValueObjects;

namespace ClassLibrary1.Services
{
    public static class AzureTableStorageService
    {
        public static async Task<Packet> IO(Packet packet)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"packet={packet}");

            await Write(packet);

            Tag.Why("PostInputCall");

            packet.Sent = true;

            Tag.Why("PreOuput");

            var receivedPacket = await Read(packet.Id);

            Tag.Line($"receivedPacket={receivedPacket}");

            Tag.Why("PostOuput");

            packet.Received = true;

            packet.Color = receivedPacket.Color;

            Tag.Why("IOComplete");

            return packet;
        }

        public static async Task Write(Packet packet)
        {
            Tag.Where("Write");

            Tag.Why("WriteStart");

            Tag.ToDo("ReadFromSecrets");
            GetAzureStorageAccountCredential(out string storageUri, out string accountName, out string storageAccountKey);

            //var serviceClient = new TableServiceClient(
            //    new Uri(storageUri),
            //    new TableSharedKeyCredential(accountName, storageAccountKey));

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
            await tableClient.AddEntityAsync<PacketTableEntity>(packetTableEntity);

            Tag.Why("WriteComplete");
        }

        private static void GetAzureStorageAccountCredential(out string storageUri, out string accountName, out string storageAccountKey)
        {
            // TODO - Obscure with Azure Traffic Manager
            storageUri = "";
            accountName = "";
            storageAccountKey = "";
        }

        public static async Task<Packet> Read(Guid id)
        {
            Tag.Where("Read");

            Tag.Why("ReadStart");

            Tag.What($"id={id}");

            Tag.ToDo("ReadFromSecrets");
            GetAzureStorageAccountCredential(out string storageUri, out string accountName, out string storageAccountKey);

            //var serviceClient = new TableServiceClient(
            //    new Uri(storageUri),
            //    new TableSharedKeyCredential(accountName, storageAccountKey));

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

            var entity = await tableClient.GetEntityAsync<PacketTableEntity>(id.ToString(), id.ToString());

            Packet packet = new()
            {
                Id = Guid.Parse(entity.Value.Id),
                Color = PacketColor.From(entity.Value.Color)
            };

            Tag.Why("ReadComplete");

            return packet;
        }
    }
}