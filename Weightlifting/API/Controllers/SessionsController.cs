using API.Data.Managers.Interfaces;
using API.Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DTO.Sessions;
using API.Data.Managers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SessionsController : Controller
    {
        private readonly ISessionsManager _sessionsManager;
        private readonly IAthletesManager _athletesManager;

        public SessionsController(ISessionsManager sessionsManager, IAthletesManager athletesManager)
        {
            _sessionsManager = sessionsManager;
            _athletesManager = athletesManager;
        }

        [HttpPost("Add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> Add([FromBody] AddSessionDTO addSessionDTO)
        {
            // Check the logged in user's identity
            var userId = User?.Identity?.Name;

            if (userId is null)
            {
                return BadRequest(new AddSessionResponseDTO()
                {
                    Success = false,
                    Errors = new List<string>() { "Error accessing user identity" }
                });
            }

            int athleteId;

            // Their user is valid, but are they an athlete?
            if (!_athletesManager.GetAthleteId(userId, out athleteId))
            {
                return BadRequest(new AddSessionResponseDTO()
                {
                    Success = false,
                    Errors = new List<string>() { "No athleteId given, and user is not an Athlete" }
                });
            }

            var result = await _sessionsManager.AddSession(athleteId, addSessionDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Details/{sessionId:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int sessionId)
        {
            var result = await _sessionsManager.Details(sessionId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Details/Edit")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> EditDetails([FromBody] EditSessionDetailsDTO editSessionDetailsDTO)
        {
            var result = await _sessionsManager.EditDetails(editSessionDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("AthleteSessions")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AthleteSessions()
        {
            // Check the logged in user's identity
            var userId = User?.Identity?.Name;

            if (userId is null)
            {
                return BadRequest(new AddSessionResponseDTO()
                {
                    Success = false,
                    Errors = new List<string>() { "Error accessing user identity" }
                });
            }

            int athleteId;

            // Their user is valid, but are they an athlete?
            if (!_athletesManager.GetAthleteId(userId, out athleteId))
            {
                return BadRequest(new AddSessionResponseDTO()
                {
                    Success = false,
                    Errors = new List<string>() { "No athleteId given, and user is not an Athlete" }
                });
            }

            var result = await _sessionsManager.GetSessions(athleteId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Delete/{sessionId:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public async Task<IActionResult> Delete(int sessionId)
        {
            var result = await _sessionsManager.Delete(sessionId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
