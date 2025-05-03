using Microsoft.AspNetCore.Mvc;

namespace Tixxp.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/Unexpected")]
        public IActionResult Unexpected()
        {
            return View();
        }
    }
}
