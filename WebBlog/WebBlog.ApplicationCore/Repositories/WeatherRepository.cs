using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebBlog.ApplicationCore.Entities;

namespace WebBlog.ApplicationCore.Repositories
{
    public class WeatherRepository : ISomeServiceRepository<Weather>
    {
        HttpClient httpClient;
        public WeatherRepository()
        {
            httpClient = new HttpClient();
        }

        public async Task<Weather> GetData(string ip)
        {
            // Didn't returned location
            var requestString = "https://openweathermap.org/data/2.5/find?q=" + /*"Vitebsk,BY"*/(await GetLocation(ip)) +
                "&type=like&sort=population&cnt=30&appid=b6907d289e10d714a6e88b30761fae22";
            var result = await httpClient.GetStringAsync(requestString);
            var objects = JsonConvert.DeserializeObject<RootObject>(result);
            var weather = new Weather()
            {
                CityName = objects?.Weathers[0]?.CityName,
                KelvinTemperature = objects.Weathers[0].Temperature.TempTemperature,
                Description = objects.Weathers[0].States[0].Description
            };
            return weather;
        }

        private class RootObject
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
