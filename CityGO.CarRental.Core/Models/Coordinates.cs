using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Coordinates
    {
        [JsonProperty("x")]
        public double X { get; set; }
        
        [JsonProperty("y")]
        public double Y { get; set; }
    }
}