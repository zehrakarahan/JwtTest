using System.Net.Http.Headers;

namespace jwt.webuı
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["AccessToken"];


            if (!string.IsNullOrEmpty(token))
            {
                // Token'ın süresini kontrol et
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                if (jwtToken.ValidTo < DateTime.UtcNow)
                {
                    // Token süresi dolmuş, login sayfasına yönlendir
                    context.Response.Cookies.Delete("AccessToken");
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }
            else
            {
                // Token yoksa login sayfasına yönlendir
                if (!context.Request.Path.Value.StartsWith("/Home"))
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }

            await _next(context);
        }
    }
}
