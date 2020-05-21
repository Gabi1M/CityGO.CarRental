using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Coordinates
    {
        [JsonProperty("X")]
        public double X { get; }
        
        [JsonProperty("Y")]
        public double Y { get; }

        //===========================================================//
        public Coordinates(double x, double y)
        {
            X = x;
            Y = y;
        }

        //===========================================================//
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}