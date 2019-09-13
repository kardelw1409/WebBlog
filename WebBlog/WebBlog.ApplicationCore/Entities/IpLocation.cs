using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebBlog.ApplicationCore.Entities
{
    public class IpLocation
    {
        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        public static async Task<IpLocation> QueryGeographicalLocationAsync(string ipAddress)
        {
            HttpClient client = new HttpClient();
            
            string result = await client.GetStringAsync("http://api.ipstack.com/" + ipAddress + "?access_key=f4c0f6818e80d66d63093866ea6ff810&output=json&legacy=1");
            var weather = JsonConvert.DeserializeObject<IpLocation>(result);
            if (weather.City == null)
            {
                throw new InvalidOperationException("City not found.");
            }
            return weather;
        }

    }
}
