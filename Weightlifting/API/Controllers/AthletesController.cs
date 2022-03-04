using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;
using API.DTOs.Athletes;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AthletesController : Controller
    {
        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public IActionResult Check()
        {
            return Ok("You're an athlete!");
        }

        [HttpPost("session/add")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Athlete)]
        public IActionResult AddSession(AddSessionDTO addSessionDTO)
        {
            var id = User.Identity.Name;

            if (id is null)
            {
                return BadRequest("Error accessing identity");
            }


            
            var session = new Session()
            {
                Date = addSessionDTO.Date
            };

            return Ok();
        }
    }
}
