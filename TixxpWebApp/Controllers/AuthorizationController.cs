using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.WebApp.Models.Authorization;

namespace Tixxp.WebApp.Controllers;
public class AuthorizationController : Controller
{
    private readonly IPersonnelService _personnelService;
    private readonly IStringLocalizer<AuthorizationController> _stringLocalizer;

    public AuthorizationController(IPersonnelService personnelService, IStringLocalizer<AuthorizationController> stringLocalizer)
    {
        _personnelService = personnelService;
        _stringLocalizer = stringLocalizer;
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
                return BadRequest(new { message = _stringLocalizer["authorizationController.LOGIN.LOGIN_FAILED"].ToString()});


            return Ok(new
            {
                message = _stringLocalizer["authorizationController.LOGIN.LOGIN_SUCCESFUL"].ToString(),
                redirectUrl = "/Home/Index"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = _stringLocalizer["authorizationController.LOGIN.SERVER_ERROR"].ToString(),
                error = ex.Message
            });
        }
    }
    #endregion

    #region ResetPassword
    public IActionResult ResetPassword()
    {
        return View("ResetPassword");
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Authorization");
    }

    [HttpGet]
    public async Task<IActionResult> LogoutGet()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Authorization");
    }
}
