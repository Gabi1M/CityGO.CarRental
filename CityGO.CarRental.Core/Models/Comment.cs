using System;
using Newtonsoft.Json;

namespace CityGO.CarRental.Core.Models
{
    public class Comment : BaseEntity
    {
        [JsonProperty("Mail")]
        public string Mail { get; }

        [JsonProperty("Content")]
        public string Content { get; }

        [JsonProperty("Datetime")]
        public DateTime DateTime { get; }

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
