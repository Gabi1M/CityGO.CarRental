using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Photo : BaseEntity
    {
        [JsonProperty("CarId")]
        public long? CarId { get; }
        
        [JsonProperty("Path")]
        public string Path { get; }

        //===========================================================//
        public Photo(long carId, string path, long? id = null)
        {
            Id = id;
            CarId = carId;
            Path = path;
        }

        //===========================================================//
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
