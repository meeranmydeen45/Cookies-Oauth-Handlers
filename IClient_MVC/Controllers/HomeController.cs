using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IClient_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpContext? context;
        private readonly HttpClient client;

        public HomeController(IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory)
        {
            context = accessor.HttpContext;
            client = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        
        
        {
            var token = context.GetTokenAsync("access_token").Result;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var res = await client.GetAsync("https://localhost:4000/secret");
            return View();
        }
    }
}