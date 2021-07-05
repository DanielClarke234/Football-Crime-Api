using Football_Crime_Api.DAL.Postcode;
using Football_Crime_Api.DAL.Teams;
using Football_Crime_Api.Models.FootballTeams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Crime_Api.Process
{
    public interface IFootballCrimeProcessor
    {
        List<FootballTeamsModel> GetTeamsAndCrimes();
    }

    public class FootballCrimeProcessor : IFootballCrimeProcessor
    {
        private readonly ITeamsLookup _teamsLookup;
        private readonly IPostcodeLookup _postcodeLookup;
        public FootballCrimeProcessor(ITeamsLookup teamsLookup, IPostcodeLookup postcodeLookup)
        {
            _teamsLookup = teamsLookup;
            _postcodeLookup = postcodeLookup;
        }

        public List<FootballTeamsModel> GetTeamsAndCrimes()
        {
            //We will want to initially get a list of all teams in the given competition id
            var teams = _teamsLookup.GetTeamsInComp();

            //We then want to loop through each team, get it's postcode from it's address and call to the postcode lookup to get its lat and long
            foreach (var team in teams)
            {
                var postcodeModel = _postcodeLookup.GetPostcodeDetails(team.postcode);
            }

            return teams;
        }
    }
}
