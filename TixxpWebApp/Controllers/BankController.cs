using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Entities.Bank;

namespace Tixxp.WebApp.Controllers
{
    public class BankController : Controller
    {
        private readonly IBankService _bankService;
        private readonly ICurrentUser _currentUser;
        private readonly IStringLocalizer<BankController> _localizer;

        public BankController(
            IBankService bankService,
            ICurrentUser currentUser,
            IStringLocalizer<BankController> localizer)
        {
            _bankService = bankService;
            _currentUser = currentUser;
            _localizer = localizer;
        }

        // --------------------------------------------------------------------
        // INDEX
        // --------------------------------------------------------------------
        public IActionResult Index()
        {
            var result = _bankService.GetAll();

            if (!result.Success)
            {
                ViewBag.ErrorMessage = _localizer["bankController.ERROR_LOAD"].Value.ToString();
                return View(new List<BankEntity>());
            }

            return View(result.Data);
        }

        // --------------------------------------------------------------------
        // SAVE (ADD + UPDATE)
        // --------------------------------------------------------------------
        [HttpPost]
        public IActionResult Save(BankEntity model)
        {
            if (model.Id > 0)
            {
                // UPDATE
                model.UpdatedBy = _currentUser.UserId ?? 0;
                model.Updated_Date = DateTime.Now;

                var result = _bankService.Update(model);

                return Json(new
                {
                    success = result.Success,
                    message = result.Success
                        ? _localizer["bankController.UPDATE_SUCCESS"].Value.ToString()
                        : _localizer["bankController.UPDATE_FAIL"].Value.ToString()
            });
            }
            else
            {
                // ADD
                model.CreatedBy = _currentUser.UserId ?? 0;
                model.Created_Date = DateTime.Now;
                model.IsDeleted = false;

                var result = _bankService.Add(model);

                return Json(new
                {
                    success = result.Success,
                    message = result.Success
                        ? _localizer["bankController.ADD_SUCCESS"].Value.ToString()
                        : _localizer["bankController.ADD_FAIL"].Value.ToString()
                });
            }
        }

        // --------------------------------------------------------------------
        // DELETE
        // --------------------------------------------------------------------
        [HttpPost]
        public IActionResult Delete(long id)
        {
            var entityRes = _bankService.GetById(id);

            if (!entityRes.Success || entityRes.Data == null)
            {
                return Json(new
                {
                    success = false,
                    message = _localizer["bankController.ERROR_NOT_FOUND"].Value.ToString()
                });
            }

            var entity = entityRes.Data;
            entity.IsDeleted = true;
            entity.UpdatedBy = _currentUser.UserId ?? 0;
            entity.Updated_Date = DateTime.Now;

            var result = _bankService.Update(entity);

            return Json(new
            {
                success = result.Success,
                message = result.Success
                    ? _localizer["bankController.DELETE_SUCCESS"].Value.ToString()
                    : _localizer["bankController.DELETE_FAIL"].Value.ToString()
            });
        }
    }
}
