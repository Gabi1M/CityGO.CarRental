using Newtonsoft.Json;
using CityGO.CarRental.Core.Enums;

namespace CityGO.CarRental.Core.Models
{
    public class Car : BaseEntity
    {
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; }
        
        [JsonProperty("Model")]
        public string Model { get; }
        
        [JsonProperty("NumberOfSeats")]
        public int NumberOfSeats { get; }
        
        [JsonProperty("Price")]
        public int Price { get; }
        
        [JsonProperty("Coordinates")]
        public Coordinates Coordinates { get; }

        [JsonProperty("State")]
        public CarState State { get; }

        //===========================================================//
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

        //===========================================================//
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Manufacturer))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Model))
            {
                return false;
            }

            if (NumberOfSeats <= 0)
            {
                return false;
            }

            if (Price <= 0)
            {
                return false;
            }

            return true;
        }

        //===========================================================//
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
