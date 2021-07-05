using AND.Models.Exceptions;
using Football_Crime_Api.DAL.Crime;
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
        private readonly ICrimesLookup _crimesLookup;

        public FootballCrimeProcessor(ITeamsLookup teamsLookup, IPostcodeLookup postcodeLookup, ICrimesLookup crimesLookup)
        {
            _teamsLookup = teamsLookup;
            _postcodeLookup = postcodeLookup;
            _crimesLookup = crimesLookup;
        }

        public List<FootballTeamsModel> GetTeamsAndCrimes()
        {
            //We will want to initially get a list of all teams in the given competition id
            var teams = _teamsLookup.GetTeamsInComp();

            //We then want to loop through each team, get it's postcode from it's address and call to the postcode lookup to get its lat and long
            foreach (var team in teams)
            {
                //Get the postcode details for the team
                var postcodeModel = _postcodeLookup.GetPostcodeDetails(team.postcode);

                //If we don't get gps data from the callback then we will need to throw
                if (postcodeModel.result.latitude == null || postcodeModel.result.longitude == null)
                {
                    throw new UserException("Could not get latitude and longitude details for all teams");
                }

                //If we have had a valid postcode model returned then we can add it to the team model
                team.postcodeRequest = postcodeModel.result;

                //From each lat long we can then call and get the crimes near that gps location and assign then to the return model
                var crimes = _crimesLookup.GetCrimesFromGps(team.postcodeRequest.latitude.Value, team.postcodeRequest.longitude.Value);

                //Add the ordered crimes to the return model
                team.crimes = crimes.OrderByDescending(_ => _.dateTime).ToList();
            }

            return teams;
        }
    }
}
