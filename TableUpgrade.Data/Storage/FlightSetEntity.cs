using System;
using Azure;
using Azure.Data.Tables;

namespace TableUpgrade.Data.Storage
{
    public class FlightSetEntity : ITableEntity
    {
        public string Airport { get; set; }

        public string FlightSetJson { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
