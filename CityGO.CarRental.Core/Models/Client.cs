using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Client : BaseEntity
    {
        [JsonProperty("Name")]
        public string Name { get; }
        
        [JsonProperty("Mail")]
        public string Mail { get; }
        
        [JsonProperty("Password")]
        public string Password { get; }
        
        [JsonProperty("NumberOfPastRentals")]
        public int NumberOfPastRentals { get; }

        //===========================================================//
        public Client(string name, string mail, string password, int numberOfPastRentals, long? id = null)
        {
            Id = id;
            Name = name;
            Mail = mail;
            Password = password;
            NumberOfPastRentals = numberOfPastRentals;
        }

        //===========================================================//
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Mail))
            {
                return false;
            }

            if (!Mail.Contains("@"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }

            return false;
        }

        //===========================================================//
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
