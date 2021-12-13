using Azure.Data.Tables;

namespace ClassLibrary1
{
    public static class AzureTableStorageService
    {
        public static async Task<Photon> IO(Photon photon)
        {
            Tag.Where("IO");

            Tag.Why("IOStart");

            Tag.Why("PreInputCall");

            Tag.What($"photon={photon}");

            await Write(photon);

            Tag.Why("PostInputCall");

            photon.Sent = true;

            Tag.Why("PreOuput");

            var receivedPhoton = await Read(photon.Id);

            Tag.What($"receivedPhoton={receivedPhoton}");

            Tag.Why("PostOuput");

            photon.Received = true;

            photon.Color = receivedPhoton.Color;

            Tag.Why("IOComplete");

            return photon;
        }

        public static async Task Write(Photon photon)
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

            var photonTableEntity = new PhotonTableEntity
            {
                Id = photon.Id.ToString(),
                ReferenceId = photon.ReferenceId.ToString(),
                Color = photon.Color.Code,
                PartitionKey = photon.Id.ToString(),
                RowKey = photon.Id.ToString(),
            };

            //tableClient.AddEntity(strongEntity);
            await tableClient.AddEntityAsync<PhotonTableEntity>(photonTableEntity);

            Tag.Why("WriteComplete");
        }

        private static void GetAzureStorageAccountCredential(out string storageUri, out string accountName, out string storageAccountKey)
        {
            // TODO - Obscure with Azure Traffic Manager
            storageUri = "";
            accountName = "";
            storageAccountKey = "";
        }

        public static async Task<Photon> Read(Guid id)
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

            var entity = await tableClient.GetEntityAsync<PhotonTableEntity>(id.ToString(), id.ToString());

            Photon photon = new()
            {
                Id = Guid.Parse(entity.Value.Id),
                Color = Color.From(entity.Value.Color)
            };

            Tag.Why("ReadComplete");

            return photon;
        }
    }
}