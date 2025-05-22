using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Entities.Counter;

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
            var result = _counterService.GetAll();
            if (result.Success)
            {
                return View(result.Data); 
            }
            return View(new List<Tixxp.Entities.Counter.CounterEntity>());
        }

        [HttpPost]
        public IActionResult Update(CounterEntity model)
        {
            var result = _counterService.Update(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _counterService.GetById(id);
            if (result.Success && result.Data != null)
                return Json(result.Data);

            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var counter = _counterService.GetById(id);
            if (!counter.Success || counter.Data == null)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            var entity = counter.Data;
            entity.IsDeleted = true;

            var result = _counterService.Update(entity);
            return Json(new { success = result.Success, message = result.Message });
        }

    }
}
