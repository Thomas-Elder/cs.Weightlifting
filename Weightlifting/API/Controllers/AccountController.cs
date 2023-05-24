using API.Data.Managers.Interfaces;

using DTO.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpGet("check")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Check()
        {
            return Ok("Authorized");
        }

        [HttpPost("register/athlete")]
        public async Task<IActionResult> RegisterAthlete([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var result = await _accountManager.RegisterAthlete(userRegistrationDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("register/coach")]
        public async Task<IActionResult> RegisterCoach([FromBody] UserRegistrationDTO userRegistrationDTO)
        {
            var result = await _accountManager.RegisterCoach(userRegistrationDTO);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationDTO userAuthenticationDTO)
        {
            var result = await _accountManager.Login(userAuthenticationDTO);

            if (!result.Success)
            {
                return BadRequest("Authentication failed");
            }

            return Ok(result);
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete([FromBody] string email)
        {
            var result = await _accountManager.Delete(email);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
