using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;
using API.Data.Managers.Interfaces;
using API.DTOs.Coaches;

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

        /// <summary>
        /// Adds the given athleteId to the logged in user.
        /// </summary>
        /// Takes an int athleteId and attempts to add this athleteId to the Coach's athletes.
        /// If there is an error accessing the User.Identity, this will return a BadRequest with 
        /// an AddAthleteToCoachResponseDTO with Success false.
        /// If the logged in users' applicationUserId is not associated with a Coach entity, 
        /// this will return a BadRequest with an AddAthleteToCoachResponseDTO with Success false.
        /// If the logged in user is a Coach, and the Athlete is successfully added to the Coach, 
        /// this will return an Ok with an AddAthleteToCoachResponseDTO with Success true.
        /// <param name="athleteId"></param>
        /// <returns></returns>
        [HttpPost("athletes/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public async Task<IActionResult> AddAthlete(int athleteId)
        {
            var applicationUserId = User?.Identity?.Name;

            if (applicationUserId is null)
            {
                return BadRequest(new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Identity Error", "Error accessing user identity" }
                    }
                });
            }

            int coachId;

            if (!_coachesManager.GetCoachId(applicationUserId, out coachId))
            {
                return BadRequest(new AddAthleteToCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach Id Error", "No Coach exists with that application user id" }
                    }
                });
            }

            var result = await _coachesManager.AddAthleteToCoach(coachId, athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/coachId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int coachId)
        {
            var result = await _coachesManager.Details(coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/coachId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MyDetails()
        {
            // Check the logged in user's identity
            var applicationUserId = User?.Identity?.Name;

            if (applicationUserId is null)
            {
                return BadRequest(new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Identity Error", "Error accessing user identity" }
                    }
                });
            }

            int coachId;

            // Check they're a coach
            if (!_coachesManager.GetCoachId(applicationUserId, out coachId))
            {
                return BadRequest(new CoachDetailsResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Coach ID Error", "No coachId given, and user is not an Coach" }
                    }
                });
            }

            // Otherwise we've gotten the coachId associated with the logged in user.
            var result = await _coachesManager.Details(coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates the details of the coach.
        /// </summary>
        /// If the given coachId is
        /// <param name="coachId"></param>
        /// <param name="editDetailsDTO"></param>
        /// <returns></returns>
        [HttpGet("details/edit/coachId")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public async Task<IActionResult> EditDetails(int coachId, EditDetailsDTO editDetailsDTO)
        {
            var result = await _coachesManager.EditDetails(coachId, editDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("delete/coachId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(int coachId = 0)
        {
            // If coachId is still default value
            if (coachId == 0)
            {
                // Then we'll check the logged in user's identity
                var applicationUserId = User?.Identity?.Name;

                if (applicationUserId is null)
                {
                    return BadRequest(new DeleteCoachDTO()
                    {
                        Success = false,
                        Errors = new Dictionary<string, string>()
                        {
                            { "Identity Error", "Error accessing user identity" }
                        }
                    });
                }

                // And they're not an coach return bad request
                if (!_coachesManager.GetCoachId(applicationUserId, out coachId))
                {
                    return BadRequest(new DeleteCoachDTO()
                    {
                        Success = false,
                        Errors = new Dictionary<string, string>()
                        {
                            { "Coach ID Error", "No coachId given, and user is not an Coach" }
                        }
                    });
                }
            }

            // Otherwise now we've either got an coachId from the parameter, or we've gotten the 
            // coachId associated with the logged in user.
            var result = await _coachesManager.Delete(coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
