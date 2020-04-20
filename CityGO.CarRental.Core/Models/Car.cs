using Newtonsoft.Json;
using CityGO.CarRental.Core.Enums;

namespace CityGO.CarRental.Core.Models
{
    public class Car : BaseEntity
    {
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; }
        
        [JsonProperty("model")]
        public string Model { get; }
        
        [JsonProperty("numberofseats")]
        public int NumberOfSeats { get; }
        
        [JsonProperty("price")]
        public int Price { get; }
        
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; }

        [JsonProperty("state")]
        public CarState State { get; }

        public Car(string manufacturer, string model, int numberOfSeats, int price, Coordinates coordinates, CarState state, long? id = null)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            NumberOfSeats = numberOfSeats;
            Price = price;
            State = state;
            Coordinates = coordinates;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
