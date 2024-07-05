using jwt.webuı.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace jwt.webuı.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(string username, string password)
		{
			// Basit bir doğrulama ve token alma işlemi (gerçek uygulamada bu yöntem farklı olabilir)
			if (username == "user" && password == "password")
			{
				using var client = new HttpClient();
				var response = await client.PostAsync("https://localhost:7200/api/Auth/token", null);
				if (response.IsSuccessStatusCode)
				{
					var token = await response.Content.ReadAsStringAsync();
					HttpContext.Response.Cookies.Append("AccessToken", token);
					return RedirectToAction("Deneme", "Home");
				}
			}

            return RedirectToAction("Index", "Home");
        }
		[HttpGet]
        public async Task<IActionResult> Deneme()
		{
            using var client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7200/api/Auth/DenemeTest");
			if (response.IsSuccessStatusCode)
			{
				var token = await response.Content.ReadAsStringAsync();

				return View();
			}
			return View();
		}
		public IActionResult Logout()
		{
			HttpContext.Response.Cookies.Delete("AccessToken");
			return RedirectToAction("Login");
		}
	}
}
