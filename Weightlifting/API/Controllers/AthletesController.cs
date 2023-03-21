using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;
using API.DTOs.Athletes;
using API.Data.Managers.Interfaces;

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
            var applicationUserId = User.Identity.Name;

            if (applicationUserId is null)
            {
                return BadRequest(new AddCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Identity Error", "Error accessing user identity" }
                    }
                });
            }

            int athleteId = 0;

            if (!_athletesManager.GetAthleteId(applicationUserId, out athleteId))
            {
                return BadRequest(new AddCoachResponseDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                    {
                        { "Identity Error", "No athlete exists with that ApplicationUserId" }
                    }
                });
            }

            var result = await _athletesManager.AddCoach(athleteId, coachId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int athleteId)
        {
            var result = await _athletesManager.Details(athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("mydetails")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> MyDetails()
        {
            // Check the logged in user's identity
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

            int athleteId;

            // Their user is valid, but are they an athlete?
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

            // Otherwise we've gotten the athleteId associated with the logged in user.
            var result = await _athletesManager.Details(athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("details/edit")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditDetails(EditDetailsDTO editDetailsDTO, int athleteId = 0)
        {
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

            // If athleteId is the default value, then we want to access the User's Athlete
            if (athleteId == 0)
            {
                // And if they're not an Athlete, bad request
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

            // And last check, if they've provided an athleteId, AND they are a logged in athlete, 
            // but the athleteId passed in isn't the athleteId of the logged in user, we need 
            // to return a badrequest.
            _athletesManager.GetAthleteId(userId, out int usersAthleteId);

            if (usersAthleteId != athleteId)
            {
                return BadRequest(new AthleteDetailsDTO()
                {
                    Success = false,
                    Errors = new Dictionary<string, string>()
                        {
                            { "Athlete ID", "The athleteId passed is not the logged in user's athleteId" }
                        }
                });
            }

            // Otherwise now we've either got an athleteId from the parameter, or we've gotten the 
            // athleteId associated with the logged in user.
            var result = await _athletesManager.EditDetails(athleteId, editDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete(int athleteId = 0)
        {

            // If athleteId is still default value
            if (athleteId == 0)
            {
                // Then we'll check the logged in user's identity
                var userId = User.Identity.Name;

                if (userId is null)
                {
                    return BadRequest(new DeleteAthleteDTO()
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
                    return BadRequest(new DeleteAthleteDTO()
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
            var result = await _athletesManager.Delete(athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
