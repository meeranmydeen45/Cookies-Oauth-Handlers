using CookieAPI.Attribute;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace CookieAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly HttpContext? httpContext;
        public HomeController(IHttpClientFactory clientFactory, IHttpContextAccessor accessor)
        {
            client = clientFactory.CreateClient();
            httpContext = accessor.HttpContext;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Home Page");
        }

        [HttpGet("Secret")]
        [Authorize(Roles ="Admin", Policy = "depPolicy")]
        public IActionResult Secret()
        {
            return Ok("Secret Page");
        }

        [Authorize]
        [HttpGet("Secret2")]
        public IActionResult Secret2()
        {
            var res = httpContext?.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
            return Ok(res);
        }
        
  
        [HttpGet("teenage")]
        [Authorize(Policy = "minimumAgeRequired")]
        public IActionResult Teenage()
        {
            return Ok("Hi Teen; You have access to the resource!!");
        }

        [HttpGet("seniors")]
        [MinimumAgeAuthorize(60)]
        public IActionResult Seniors()
        {
            return Ok("Hi Seniors; You have access to the resource!!");
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login()
        {
            var myCalims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Meeran"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Email, "meeran@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth, "05/04/1950"),
                new Claim("DPT", "IT"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims: myCalims, "MySecretClaims");
            ClaimsPrincipal principal = new ClaimsPrincipal(new[] { claimsIdentity });

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,

            };

        //    Response.Cookies.Append("MyCookieName", "cookieValue", new CookieOptions
        //    {
        //        Path = "/",
        //        Domain = httpContext.Request.Host.Host,
        //        IsEssential= true,
        //        SameSite = SameSiteMode.None,

        //}) ;

            await httpContext?.SignInAsync("Cookies", principal: principal, authProperties);


            //if (!string.IsNullOrEmpty(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}
            return Ok("LoginSucessfully");
        }
    }
}