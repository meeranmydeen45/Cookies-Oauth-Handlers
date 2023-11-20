using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Server Secret Page");
        }
    }
}
