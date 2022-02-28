using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;

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
    }
}
