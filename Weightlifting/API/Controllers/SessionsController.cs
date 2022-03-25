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

        [HttpPost("details")]
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

        [HttpPost("details/edit")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditDetails(EditSessionDetailsDTO editSessionDetailsDTO)
        {
            var result = await _sessionsManager.EditDetails(editSessionDetailsDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
