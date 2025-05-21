using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Counter;

namespace Tixxp.WebApp.Controllers
{
    [Authorize]
    public class CounterController : Controller
    {
        private readonly ICounterService _counterService;

        public CounterController(ICounterService counterService)
        {
            _counterService = counterService;
        }

        public IActionResult Index()
        {
            var aa = _counterService.GetAll();
            return View();
        }
    }
}
