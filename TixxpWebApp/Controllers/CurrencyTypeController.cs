using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Entities.CurrencyType;

namespace Tixxp.WebApp.Controllers
{
    public class CurrencyTypeController : Controller
    {
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IStringLocalizer<CurrencyTypeController> _stringLocalizer;

        public CurrencyTypeController(
            ICurrencyTypeService currencyTypeService,
            IStringLocalizer<CurrencyTypeController> stringLocalizer)
        {
            _currencyTypeService = currencyTypeService;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var result = _currencyTypeService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }

            return View(new List<Tixxp.Entities.CurrencyType.CurrencyTypeEntity>());
        }

        [HttpPost]
        public IActionResult Save(CurrencyTypeEntity model)
        {
            if (model.Id > 0)
            {
                var result = _currencyTypeService.Update(model);
                return Json(new { success = result.Success, message = result.Message });
            }
            else
            {
                model.CreatedBy = 6;
                model.Created_Date = DateTime.Now;
                var result = _currencyTypeService.Add(model);
                return Json(new { success = result.Success, message = result.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var entity = _currencyTypeService.GetById(id).Data;
            if (entity == null)
                return Json(new { success = false, message = _stringLocalizer["currencyType.INDEX.RECORD_NOT_FOUND"].Value });

            entity.IsDeleted = true;
            var result = _currencyTypeService.Update(entity);
            return Json(new { success = result.Success, message = result.Message });
        }

    }
}
