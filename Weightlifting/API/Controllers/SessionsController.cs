using API.Data.Managers.Interfaces;
using API.DTOs.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class SessionsController : Controller
    {
        private readonly ISessionsManager _sessionsManager;

        public SessionsController(ISessionsManager sessionsManager)
        {
            _sessionsManager = sessionsManager;
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Add(AddSessionDTO addSessionDTO)
        {
            var result = await _sessionsManager.AddSession(addSessionDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
