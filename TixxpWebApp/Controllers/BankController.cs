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
    }
}
