using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Core.Utilities.Results.Abstract;
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
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            try
            {
                IDataResult<LoginResponseDto> loginResult = await _personnelService.Login(new LoginRequestDto
                {
                    Email = loginRequestModel.Username,
                    Password = loginRequestModel.Password
                });

                if (!loginResult.Success)
                    return BadRequest(new { message = loginResult.Message ?? "Giriş başarısız." });

                return Ok(new { message = loginResult.Message ?? "Giriş başarılı.", redirectUrl = "/Home/Index" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Sunucu hatası oluştu.",
                    error = ex.Message
                });
            }
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Authorization");
        }
    }
}
