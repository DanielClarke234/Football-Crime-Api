using Football_Crime_Api.Models.Crime;
using Football_Crime_Api.Models.PostcodeLookup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Crime_Api.Models.FootballTeams
{
    public class FootballTeamsModel
    {
        public string name { get; set; }
        public string shortName { get; set; }
        public string tla { get; set; }
        public string crestUrl { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public string email { get; set; }
        public int founded { get; set; }
        [JsonProperty("clubColors")]
        public string clubColours { get; set; }
        public string venue { get; set; }
        public string lastUpdated { get; set; }

        public PostcodeDetailsModel postcodeRequest { get; set; }
        public List<CrimeDetailsModel> crimes { get; set; }

        //Get the postcode from the data we get back from the football API call
        public string postcode => !string.IsNullOrEmpty(address) ? address.Substring(address.Substring(0, address.LastIndexOf(' ')).LastIndexOf(' ') + 1) : "";
    }
}
