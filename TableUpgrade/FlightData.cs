using System;
using System.Threading.Tasks;
using TableUpgrade.Data.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TableUpgrade.Data.JsonResult;
using System.Diagnostics;
using Azure.Data.Tables;

namespace TableUpgrade
{
    public static class FlightData
    {
        [FunctionName("FlightData")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            [Table("FlightDataCache", "AzureWebJobsStorage")]
            TableClient resultsCacheTable)
        {

            var airport = req.Query["airport"];
            if (airport.ToString() == string.Empty)
            {
                airport = "XYZ";
            }
            var _flightCache = new FlightCache(resultsCacheTable, airport);

            var flights = await _flightCache.GetCachedFlightSet();
            if (flights == null)
            {
                flights = GetSomeRandomData();
                await _flightCache.SetCachedFlightSet(flights);
                Debug.WriteLine("New flightset");
            }
            else
            {
                flights.IsCachedValue = true;
                Debug.WriteLine("Cached flightset");
            }

            return new OkObjectResult(JsonConvert.SerializeObject(flights));
        }


        private static FlightSet GetSomeRandomData()
        {

            return new FlightSet { SomeProperty = $"Some property {new Random(25)}", 
                SomeOtherProperty = $"Some property {new Random(35)}" };
        }
    }
}
