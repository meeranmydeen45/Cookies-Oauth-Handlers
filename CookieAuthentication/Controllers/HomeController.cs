using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("CookieAuthentication!!");
        }
    }
}