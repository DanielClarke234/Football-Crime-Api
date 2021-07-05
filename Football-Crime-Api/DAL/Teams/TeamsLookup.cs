using AND.Models.Exceptions;
using Football_Crime_Api.Models.FootballTeams;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Football_Crime_Api.DAL.Teams
{
    public interface ITeamsLookup
    {
        List<FootballTeamsModel> GetTeamsInComp();
    }

    public class TeamsLookup : ITeamsLookup
    {
        private readonly IConfiguration _config;
        public TeamsLookup(IConfiguration config)
        {
            _config = config;
        }

        public List<FootballTeamsModel> GetTeamsInComp()
        {
            using (var client = new WebClient())
            {
                var compId = _config.GetValue<string>("FootballData:CompetitionId");
                client.Headers.Add("X-Auth-Token", _config.GetValue<string>("Tokens:FootballData"));
                var teamsData = client.DownloadString(new Uri(_config.GetValue<string>("URLs:FootballData") + "competitions/" + compId + "/teams"));
                var teamsModel = JsonConvert.DeserializeObject<FootballTeamsResponseModel>(teamsData);

                foreach(var team in teamsModel.teams)
                {
                    if (string.IsNullOrEmpty(team.address))
                    {
                        throw new UserException("Could not get postcode details from clubs");
                    }
                }

                return teamsModel.teams;
            }
        }
    }
}
