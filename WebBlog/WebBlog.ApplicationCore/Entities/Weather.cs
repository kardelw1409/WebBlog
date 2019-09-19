namespace WebBlog.ApplicationCore.Entities
{
    public class Weather
    {
        public string CityName { get; set; }

        public string Country { get; set;}

        public double KelvinTemperature { get; set; }
        public string Description { get; set; }
    }
}
