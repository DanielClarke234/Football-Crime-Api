using AND.Models.Exceptions;
using AND.Models.PostcodeLookup;
using Football_Crime_Api.DAL.Crime;
using Football_Crime_Api.DAL.Postcode;
using Football_Crime_Api.DAL.Teams;
using Football_Crime_Api.Models.FootballTeams;
using Football_Crime_Api.Models.PostcodeLookup;
using Football_Crime_Api.Process;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Football_Crime_Api_Tests
{
    public class FootballCrimeProcessor_GetTeamsAndCrimes
    {
        private readonly Mock<ITeamsLookup> _teamsLookup = new Mock<ITeamsLookup>();
        private readonly Mock<IPostcodeLookup> _postcodeLookup = new Mock<IPostcodeLookup>();
        private readonly Mock<ICrimesLookup> _crimesLookup = new Mock<ICrimesLookup>();

        private readonly FootballCrimeProcessor _footballCrimeProcessor; 

        public FootballCrimeProcessor_GetTeamsAndCrimes()
        {
            _footballCrimeProcessor = new FootballCrimeProcessor(_teamsLookup.Object, _postcodeLookup.Object, _crimesLookup.Object);
        }

        public static IEnumerable<object[]> PostcodeResponses =>
            new List<object[]>
            {
                //No result
                new object[] { new PostcodeLookupResponseModel()
                    {
                        result = null
                    }
                },
                //Missing lat
                new object[] { new PostcodeLookupResponseModel()
                    {
                        result = new PostcodeDetailsModel()
                        {
                            latitude = null,
                            longitude = 12
                        }
                    }
                },
                //Missing long
                new object[] { new PostcodeLookupResponseModel()
                    {
                        result = new PostcodeDetailsModel()
                        {
                            latitude = 12,
                            longitude = null
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(PostcodeResponses))]
        //Test to check that we throw when the postcode lookup does not return either a latitiude or a longitude
        public void FootballCrimeProcessor_GetTeamsAndCrimes_PostcodeNoGps(PostcodeLookupResponseModel model)
        {
            //Moq up the teams lookup to return some teams to loop through
            _teamsLookup.Setup(_ => _.GetTeamsInComp()).Returns(new List<FootballTeamsModel>()
            {
                new FootballTeamsModel()
                {

                }
            });

            //Moq up the postcode lookup to return the model
            _postcodeLookup.Setup(_ => _.GetPostcodeDetails(It.IsAny<string>())).Returns(model);

            //Check that the process throws the correct type of exception with the correct message
            var ex = Assert.Throws<UserException>(() => _footballCrimeProcessor.GetTeamsAndCrimes());

            Assert.Equal("Could not get latitude and longitude details for all teams", ex.Message);

            //Check that calls to the crime lookup were not made
            _crimesLookup.Verify(_ => _.GetCrimesFromGps(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Never);
        }
    }
}
