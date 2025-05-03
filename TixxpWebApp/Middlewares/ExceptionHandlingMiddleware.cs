using System.Text.Json;

namespace Tixxp.WebApp.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MIDDLEWARE ERROR]: {ex.Message}");

            var isApiRequest =
                context.Request.Path.StartsWithSegments("/api") ||
                context.Request.Headers["Accept"].ToString().Contains("application/json") ||
                context.Request.ContentType?.Contains("application/json") == true;

            if (isApiRequest)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var json = JsonSerializer.Serialize(new
                {
                    isSuccess = false,
                    message = "Beklenmeyen bir hata oluştu.",
                    redirectUrl = "/Error/Unexpected" // 👈 burayı dönüyoruz artık
                });

                await context.Response.WriteAsync(json);
            }
            else
            {
                context.Response.Redirect("/Error/Unexpected");
            }
        }
    }
}
