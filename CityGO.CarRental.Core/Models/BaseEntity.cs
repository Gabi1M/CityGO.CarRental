using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public abstract class BaseEntity
    {
        [JsonProperty("Id")]
        public long? Id { get; protected set; }
    }
}
