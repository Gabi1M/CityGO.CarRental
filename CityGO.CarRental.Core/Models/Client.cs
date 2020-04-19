using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Client : BaseEntity
    {
        [JsonProperty("name")]
        public string Name { get; }
        
        [JsonProperty("mail")]
        public string Mail { get; }
        
        [JsonProperty("password")]
        public string Password { get; }
        
        [JsonProperty("numberOfPastRentals")]
        public int NumberOfPastRentals { get; }

        public Client(string name, string mail, string password, int numberOfPastRentals, long? id = null)
        {
            Id = id;
            Name = name;
            Mail = mail;
            Password = password;
            NumberOfPastRentals = numberOfPastRentals;
        }
    }
}
