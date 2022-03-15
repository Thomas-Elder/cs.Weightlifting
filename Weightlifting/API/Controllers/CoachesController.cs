﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;
using API.Data.Managers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CoachesController : Controller
    {
        private readonly ICoachesManager _coachesManager;

        public CoachesController(ICoachesManager coachesManager)
        {
            _coachesManager = coachesManager;
        }

        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public IActionResult Check()
        {
            return Ok("You're a coach!");
        }

        [HttpPost("athletes/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public async Task<IActionResult> AddAthlete(int athleteId)
        {
            var coachId = User.Identity.Name;

            if (coachId is null)
            {
                return BadRequest("Error accessing identity");
            }

            var result = await _coachesManager.AddAthleteToCoach(coachId, athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/applicationId")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public async Task<IActionResult> DetailsByApplicationId()
        {
            var coachId = User.Identity.Name;

            if (coachId is null)
            {
                return BadRequest("Error accessing identity");
            }

            var result = await _coachesManager.DetailsByApplicationUserId(coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/coachId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DetailsByCoachId(int coachId)
        { 
            var result = await _coachesManager.DetailsByCoachId(coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
