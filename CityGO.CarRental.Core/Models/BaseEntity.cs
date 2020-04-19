using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public abstract class BaseEntity
    {
        [JsonProperty("id")]
        public long? Id { get; protected set; }
    }
}
