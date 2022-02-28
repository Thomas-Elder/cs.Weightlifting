using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using API.Data.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class CoachesController : Controller
    {
        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.Coach)]
        public IActionResult Check()
        {
            return Ok("You're a coach!");
        }
    }
}
