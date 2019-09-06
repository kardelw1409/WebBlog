using Newtonsoft.Json;
using WebBlog.ApplicationCore.Entities.AbstractEntities;

namespace WebBlog.ApplicationCore.Entities
{
    public class Weather : Entity
    {
        [JsonProperty("name")]
        public string SityName { get; set; }

        public string Country { get; set;}

        public double KelvinTemperature { get; set; }
        public string Description { get; set; }
    }
}
