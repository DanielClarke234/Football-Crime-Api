﻿using AND.Models.Exceptions;
using Football_Crime_Api.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Football_Crime_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FootballCrimeController : ControllerBase
    {
        private readonly ILogger<FootballCrimeController> _logger;
        private readonly IFootballCrimeProcessor _footballCrime;

        public FootballCrimeController(ILogger<FootballCrimeController> logger, IFootballCrimeProcessor footballCrime)
        {
            _logger = logger;
            _footballCrime = footballCrime;
        }

        [HttpGet]
        [Route("CrimesAtClubs")]
        public IActionResult GetAllPremierLeagueCrimes()
        {
            try
            {
                var crimes = _footballCrime.GetTeamsAndCrimes();

                return Ok(crimes);
            }
            catch (UserException ue)
            {
                return BadRequest(ue.Message);
            }
            catch (Exception e)
            {
                //Call to some error logging such as sentry.
                return BadRequest("There has been a problem that has been logged");
            }
        }
    }
}