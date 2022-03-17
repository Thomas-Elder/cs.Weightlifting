using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;
using API.DTOs.Athletes;
using API.Data.Managers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AthletesController : Controller
    {
        private readonly IAthletesManager _athletesManager;

        public AthletesController(IAthletesManager athletesManager)
        {
            _athletesManager = athletesManager;
        }

        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public IActionResult Check()
        {
            return Ok("You're an athlete!");
        }

        [HttpPost("coach/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> AddCoach(int coachId)
        {
            var id = User.Identity.Name;

            if (id is null)
            {
                return BadRequest("Error accessing identity");
            }

            var result = await _athletesManager.AddCoach(id, coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("session/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> AddSession(AddSessionDTO addSessionDTO)
        {
            var id = User.Identity.Name;

            if (id is null)
            {
                return BadRequest("Error accessing identity");
            }

            var result = await _athletesManager.AddSession(id, addSessionDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int athleteId = 0)
        {

            // If athleteId is still default value
            if (athleteId == 0)
            {
                // Then we'll check the logged in user's identity
                var userId = User.Identity.Name;

                if (userId is null)
                {
                    return BadRequest(new AthleteDetailsDTO()
                    {
                        Success = false,
                        Errors = new Dictionary<string, string>()
                        {
                            { "Identity Error", "Error accessing user identity" }
                        }
                    });
                }

                // And they're not an athlete return bad request
                if (!_athletesManager.GetAthleteId(userId, out athleteId))
                {
                    return BadRequest(new AthleteDetailsDTO()
                    {
                        Success = false,
                        Errors = new Dictionary<string, string>()
                        {
                            { "Athlete ID", "No athleteId given, and user is not an Athlete" }
                        }
                    });
                }
            }

            // Otherwise now we've either got an athleteId from the parameter, or we've gotten the 
            // athleteId associated with the logged in user.
            var result = await _athletesManager.Details(athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/edit/applicationId")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> EditDetailsByApplicationUserId(EditDetailsDTO editDetailsDTO)
        {
            var id = User.Identity.Name;

            if (id is null)
            {
                return BadRequest("Error accessing identity");
            }

            var result = await _athletesManager.EditDetailsByApplicationUserId(id, editDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/edit/athleteId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditDetailsByAthleteId(int athleteId, EditDetailsDTO editDetailsDTO)
        {
            var result = await _athletesManager.EditDetailsByAthleteId(athleteId, editDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
