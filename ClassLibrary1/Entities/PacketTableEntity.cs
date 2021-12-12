using Azure;
using Azure.Data.Tables;

namespace ClassLibrary1
{
    public class PacketTableEntity : ITableEntity
    {
        public string Id { get; set; }
        public string ReferenceId {  get; set; }
        public string Color { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
