using API.Data.Managers.Interfaces;
using API.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    { 
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok("Connection check ok");
        }
    }
}
