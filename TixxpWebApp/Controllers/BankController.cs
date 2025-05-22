using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Bank;

namespace Tixxp.WebApp.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        public IActionResult Index()
        {
            IDataResult<List<BankEntity>> result = _bankService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }
            else
            {
                return View(new List<BankEntity>());
            }
        }

        [HttpPost]
        public IActionResult Save(BankEntity model)
        {
            if (model.Id > 0)
            {
                model.CreatedBy = 6;
                model.UpdatedBy = 6;
                model.Created_Date = DateTime.Now;
                model.Updated_Date = DateTime.Now;
                var result = _bankService.Update(model);
                return Json(new { success = result.Success, message = result.Message });
            }
            else
            {
                model.CreatedBy = 6;
                model.Created_Date = DateTime.Now;
                var result = _bankService.Add(model);
                return Json(new { success = result.Success, message = result.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var entity = _bankService.GetById(id).Data;
            if (entity == null)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            entity.IsDeleted = true;
            var result = _bankService.Update(entity);
            return Json(new { success = result.Success, message = result.Message });
        }

    }
}
