using AND.Models.Exceptions;
using AND.Models.PostcodeLookup;
using Football_Crime_Api.DAL.Crime;
using Football_Crime_Api.DAL.Postcode;
using Football_Crime_Api.DAL.Teams;
using Football_Crime_Api.Models.Crime;
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

        public static IEnumerable<object[]> TeamResponses =>
            new List<object[]>
            {
                //No teams
                new object[] { new List<FootballTeamsModel>()
                    {
                        
                    }
                },
                //One Team
                new object[] { new List<FootballTeamsModel>()
                    { 
                        new FootballTeamsModel()
                    }
                },
                //Many Teams
                new object[] { new List<FootballTeamsModel>()
                    {
                        new FootballTeamsModel(),
                        new FootballTeamsModel(),
                        new FootballTeamsModel(),
                        new FootballTeamsModel(),
                        new FootballTeamsModel() 
                    }
                },
            };

        public static IEnumerable<object[]> PostcodeInvalidResponses =>
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
        [MemberData(nameof(TeamResponses))]
        //Check that we loop through the correct amount of teams
        public void FootballCrimeProcessor_GetTeamsAndCrimes_TeamLoopCheck(List<FootballTeamsModel> model)
        {
            //Moq up the teams lookup to return a given amount of teams
            _teamsLookup.Setup(_ => _.GetTeamsInComp()).Returns(model);

            //Moq the postcode lookup to give a valid response
            _postcodeLookup.Setup(_ => _.GetPostcodeDetails(It.IsAny<string>())).Returns(new PostcodeLookupResponseModel()
            {
                result = new PostcodeDetailsModel()
                {
                    latitude = 12m,
                    longitude = 12m
                }
            });

            //Moq the crime lookup to give a valid response
            _crimesLookup.Setup(_ => _.GetCrimesFromGps(It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(new List<CrimeDetailsModel>());

            var footballCrimes = _footballCrimeProcessor.GetTeamsAndCrimes();

            //the correct amount of loops is the amount of teams
            var correctLoopAmount = model.Count;

            //Check that the teams lookup was only called to once
            _teamsLookup.Verify(_ => _.GetTeamsInComp(), Times.Once());

            //Check that the correct amount of loops happened
            _postcodeLookup.Verify(_ => _.GetPostcodeDetails(It.IsAny<string>()), Times.Exactly(correctLoopAmount));
            _crimesLookup.Verify(_ => _.GetCrimesFromGps(It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Exactly(correctLoopAmount));
        }


        [Theory]
        [MemberData(nameof(PostcodeInvalidResponses))]
        //Test to check that we throw when the postcode lookup does not return either a latitiude or a longitude
        public void FootballCrimeProcessor_GetTeamsAndCrimes_PostcodeNoGps(PostcodeLookupResponseModel model)
        {
            //Moq up the teams lookup to return one team to loop through
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
