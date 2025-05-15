using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.WebApp.Models.Authorization;

namespace Tixxp.WebApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IPersonnelService _personnelService;

        public AuthorizationController(IPersonnelService personnelService)
        {
            _personnelService = personnelService;
        }

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion


        #region Login
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestModel loginRequestModel)
        {


            return Ok();
        }
        #endregion
    }
}
