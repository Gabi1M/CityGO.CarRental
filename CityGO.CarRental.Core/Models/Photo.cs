using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Photo : BaseEntity
    {
        public long? CarId { get; }
        [JsonProperty("path")]
        public string Path { get; }

        public Photo(long carId, string path, long? id = null)
        {
            Id = id;
            CarId = carId;
            Path = path;
        }
    }
}
