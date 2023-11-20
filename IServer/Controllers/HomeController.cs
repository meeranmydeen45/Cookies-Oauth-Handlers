using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly HttpContext? httpContext;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Server Public Page!!");
        }

        [HttpGet("secret")]
        [Authorize(Policy = "myDPTPolicy")]
        public IActionResult Secret()
        {
            var response = httpContext?.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
            return Ok(new { obj = response , message = "I am Secret Message"});
        }


        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Name", "David"),
                new Claim("Age", "22"),
                new Claim("DPT", "CSE"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "cookiemy");
            var claimsPricipal = new ClaimsPrincipal(new[] { claimsIdentity });
            await httpContext?.SignInAsync(claimsPricipal);


            byte[] secretBytes = Encoding.UTF8.GetBytes(Constants.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var singingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddHours(2),
                singingCredentials);

           string jsonWebToken = new JwtSecurityTokenHandler().WriteToken(token);

           return Ok(new {access_token = jsonWebToken });
        }
    }
}   