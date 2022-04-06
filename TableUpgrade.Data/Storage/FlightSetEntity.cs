using Microsoft.WindowsAzure.Storage.Table;

namespace TableUpgrade.Data.Storage
{
    public class FlightSetEntity : TableEntity
    {
        public string Airport { get; set; }

        public string FlightSetJson { get; set; }
    }
}
