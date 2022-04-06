using System.Net.Http;
using System.Threading.Tasks;
using TableUpgrade.Data.JsonResult;
using Newtonsoft.Json;
using Xunit;

namespace TableUpgrade.Test
{
    public class LocalTest
    {
        [Fact]
        public async Task CheckSingleCall()
        {
            var result = await GetCurrentFlightSet();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task CheckDoubleHit()
        {
            await Task.Delay(22000);
            var result = await GetCurrentFlightSet();
            Assert.False(result.IsCachedValue);
            await Task.Delay(500);
            result = await GetCurrentFlightSet();
            Assert.True(result.IsCachedValue);
        }

        private async Task<FlightSet> GetCurrentFlightSet()
        {
            var urlData = "http://localhost:7071/api/FlightData?airport=XYZ";

            var request = new HttpRequestMessage(HttpMethod.Get, urlData);
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FlightSet>(result);
            }
        }
    }
}
