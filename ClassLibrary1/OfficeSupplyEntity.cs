﻿using Azure;
using Azure.Data.Tables;

namespace ClassLibrary1
{
    public class OfficeSupplyEntity : ITableEntity
    {
        public string Product { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
