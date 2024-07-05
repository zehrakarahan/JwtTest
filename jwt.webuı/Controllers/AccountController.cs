using Microsoft.AspNetCore.Mvc;

namespace jwt.webuı.Controllers
{
    public class AccountController : Controller
    {
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

            return RedirectToAction("Login", "Home");
        }
    }
}
