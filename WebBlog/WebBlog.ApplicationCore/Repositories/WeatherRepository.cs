using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class WeatherRepository : ISomeServiceRepository<Weather>
    {
        IHttpContextAccessor httpContextAccessor;
        public WeatherRepository(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Weather> GetData()
        {
            var ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

            var requestString = "https://openweathermap.org/data/2.5/find?q=" + (await GetLocation(ip)) +
                "&type=like&sort=population&cnt=30&appid=b6907d289e10d714a6e88b30761fae22";

            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(requestString);

            var objects = JsonConvert.DeserializeObject<JsonResultList>(result);

            var weather = new Weather()
            {
                CityName = objects?.Weathers[0]?.CityName,
                KelvinTemperature = objects.Weathers[0].Temperature.TempTemperature,
                Description = objects.Weathers[0].States[0].Description
            };

            return weather;
        }

        private class JsonResultList
        {
            [JsonProperty("list")]
            public List<JsonWeather> Weathers { get; set; }
        }
        private class JsonWeather
        {
            [JsonProperty("name")]
            public string CityName { get; set; }
            [JsonProperty("main")]
            public Temperature Temperature { get; set; }
            [JsonProperty("weather")]
            public List<State> States { get; set; }

        }

        private class Temperature
        {
            [JsonProperty("temp")]
            public double TempTemperature { get; set; }
        }

        private class State
        {
            [JsonProperty("description")]
            public string Description { get; set; }
        }

        private async Task<string> GetLocation(string ip)
        {
            var location = await IpLocation.QueryGeographicalLocationAsync(ip);
            return location.City + "," + location.CountryCode;
        }
    }


}
