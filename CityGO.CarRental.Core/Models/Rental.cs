using System;
using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Rental : BaseEntity
    { 
       [JsonProperty("CarId")]
       public long CarId { get; set; }

       [JsonProperty("ClientID")]
       public long ClientId { get; set; }

       [JsonProperty("DateTime")]
       public DateTime DateTime { get; set; }

       //===========================================================//
       public Rental(long carId, long clientId, DateTime dateTime, long? id = null)
       {
           Id = id;
           CarId = carId;
           ClientId = clientId;
           DateTime = dateTime;
       }

       //===========================================================//
       public override string ToString()
       {
           return JsonConvert.SerializeObject(this);
       }
    }
}
