using TableUpgrade.Data.JsonResult;
using TableUpgrade.Data.Storage;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace TableUpgrade.Data.Components
{
    public class FlightCache
    {
        private readonly TableClient _flightCacheTable;
        private readonly string _airport;

        public FlightCache(TableClient flightCacheTable, string airport)
        {
            _flightCacheTable = flightCacheTable;
            _airport = airport;
        }

        public async Task<FlightSet> GetCachedFlightSet()
        {
            var cachedData = await _flightCacheTable.GetAsync<FlightSetEntity>(_airport);
            if (cachedData != null)
            {
                var duration = (DateTimeOffset.UtcNow - 
                                cachedData.Timestamp.Value.UtcDateTime).Duration();
                if (duration < TimeSpan.FromSeconds(20))
                {
                    cachedData.FlightSetJson = cachedData.FlightSetJson;
                    return JsonConvert.DeserializeObject<FlightSet>(
                        cachedData.FlightSetJson);
                }
            }
            return null;
        }

        public async Task SetCachedFlightSet(FlightSet flightSet)
        {
            var setToCache = new FlightSetEntity
            {
                Airport = _airport,
                FlightSetJson = JsonConvert.SerializeObject(flightSet),
                Timestamp = DateTimeOffset.UtcNow
            };
            await _flightCacheTable.StoreAsync(setToCache, setToCache.Airport);
        }
    }
}
