using Microsoft.AspNetCore.Mvc;

namespace Tixxp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
