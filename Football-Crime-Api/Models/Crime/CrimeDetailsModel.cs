using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Crime_Api.Models.Crime
{
    public class CrimeDetailsModel
    {
        public string category { get; set; }
        [JsonProperty("location_type")]
        public string locationType { get; set; }
        public CrimeLocationModel location { get; set; }
        public string context { get; set; }
        [JsonProperty("outcome_status")]
        public CrimeOutcomeModel outcomeStatus { get; set; }
        [JsonProperty("persistent_id")]
        public string persistentId { get; set; }
        public string id { get; set; }
        [JsonProperty("location_subtype")]
        public string locationSubtype { get; set; }
        public string month { get; set; }
        public DateTime? dateTime => !string.IsNullOrEmpty(month) ? DateTime.Parse(month) : null;
    }

    public class CrimeLocationModel
    {
        public string latitude { get; set; }
        public CrimeLocationStreetModel street { get; set; }
        public string longitude { get; set; }
    }

    public class CrimeLocationStreetModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CrimeOutcomeModel
    {
        public string category { get; set; }
        public string date { get; set; }
    }
}
