using API.Data.Managers;
using API.DTOs.Account;
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO userRegistrationDTO)
        {
            var result = await _accountManager.Register(userRegistrationDTO);

            if (!result.isSuccessfulRegistration)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserAuthenticationDTO userAuthenticationDTO)
        {
            var response = await _accountManager.Login(userAuthenticationDTO);

            if (response == null)
            {
                return BadRequest("Authentication failed");
            }

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok("Authorized");
        }
    }
}
