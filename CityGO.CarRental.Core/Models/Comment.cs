using System;
using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Comment : BaseEntity
    {
        [JsonProperty("Mail")]
        public string Mail { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Datetime")]
        public DateTime DateTime { get; set; }

        //============================================================
        public Comment(string mail, string content, DateTime dateTime, long? id = null)
        {
            Id = id;
            Mail = mail;
            Content = content;
            DateTime = dateTime;
        }

        //============================================================
        public bool Validate()
        {
            if (string.IsNullOrEmpty(Mail))
            {
                return false;
            }

            if (!Mail.Contains("@"))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Content))
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
