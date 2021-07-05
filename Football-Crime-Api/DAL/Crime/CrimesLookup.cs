using Football_Crime_Api.Models.Crime;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Football_Crime_Api.DAL.Crime
{
    public interface ICrimesLookup
    {
        List<CrimeDetailsModel> GetCrimesFromGps(decimal latitude, decimal longitude);
    }

    public class CrimesLookup : ICrimesLookup
    {
        private readonly IConfiguration _config;

        public CrimesLookup(IConfiguration config)
        {
            _config = config;
        }

        public List<CrimeDetailsModel> GetCrimesFromGps(decimal latitude, decimal longitude)
        {
            using (var client = new WebClient())
            {
                var crimesData = client.DownloadString(new Uri(_config.GetValue<string>("URLs:CrimeData") + "crimes-at-location?lat=" + latitude.ToString() + "&lng=" + longitude.ToString()));
                var crimes = JsonConvert.DeserializeObject<List<CrimeDetailsModel>>(crimesData);

                return crimes;
            }
        }
    }
}
