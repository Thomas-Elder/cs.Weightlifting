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
        private readonly AthletesManager _athletesManager;

        public AthletesController(AthletesManager athletesManager)
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
    }
}
