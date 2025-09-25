using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.Google;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.WebApp.Models.Authorization;

namespace Tixxp.WebApp.Controllers;

public class AuthorizationController : Controller
{
    private readonly IPersonnelService _personnelService;
    private readonly IAuthenticatorService _authenticatorService;
    private readonly IStringLocalizer<AuthorizationController> _stringLocalizer;

    public AuthorizationController(
        IPersonnelService personnelService,
        IAuthenticatorService authenticatorService,
        IStringLocalizer<AuthorizationController> stringLocalizer)
    {
        _personnelService = personnelService;
        _authenticatorService = authenticatorService;
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
            var loginResult = await _personnelService.Login(new LoginRequestDto
            {
                Email = loginRequestModel.Username,
                Password = loginRequestModel.Password
            });

            if (!loginResult.Success)
            {
                return BadRequest(new
                {
                    message = _stringLocalizer["authorizationController.LOGIN.LOGIN_FAILED"].ToString()
                });
            }

            var dto = loginResult.Data;

            // LoginType’a göre yönlendir
            if (dto.LoginTypeId == 1) // SMS OTP
            {
                return Ok(new
                {
                    message = _stringLocalizer["authorizationController.Login.SmsOtpVerificationRequired"].ToString(),
                    requireSmsOtp = true,
                    email = dto.Email
                });
            }
            else if (dto.LoginTypeId == 2) // Google Authenticator
            {
                return Ok(new
                {
                    message = _stringLocalizer["authorizationController.Login.GoogleVerificationRequired"].ToString(),
                    requireGoogleOtp = true,
                    email = dto.Email,
                    hasSecret = !string.IsNullOrEmpty(dto.SecretKey)
                });
            }

            // Normal login (hiç OTP istemeyenler)
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

    #region Register Authenticator
    [HttpPost]
    public IActionResult RegisterAuthenticator([FromBody] RegisterAuthenticatorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Email))
        {
            return BadRequest(new
            {
                message = _stringLocalizer["authorizationController.Login.EmailRequired"].ToString()
            });
        }

        var qrResult = _authenticatorService.RegisterAuthenticator(request.Email);

        if (!qrResult.Success || qrResult.Data == null)
        {
            return BadRequest(new
            {
                message = qrResult.Message ?? _stringLocalizer["authorizationController.AUTHENTICATOR.REGISTER_FAILED"].ToString()
            });
        }

        string base64Qr = Convert.ToBase64String(qrResult.Data);

        return Ok(new
        {
            message = _stringLocalizer["authorizationController.AUTHENTICATOR.REGISTER_SUCCESS"].ToString(),
            data = base64Qr
        });
    }
    #endregion


    #region Validate Authenticator
    [HttpPost]
    public async Task<IActionResult> ValidateAuthenticator([FromBody] ValidateAuthenticatorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request?.Otp))
        {
            return BadRequest(new
            {
                message = _stringLocalizer["authorizationController.Login.OtpCodeRequired"].ToString()
            });
        }
        var validateResult = await _authenticatorService.ValidateOtpAsync(request.Email, request.Otp);
        if (!validateResult.Success)
        {
            return BadRequest(new
            {
                message = validateResult.Message
            });
        }
        // ✅ OTP doğrulandıktan sonra burada cookie login yapılabilir
        // ör: _personnelService.FinalizeLogin(request.Email);
        return Ok(new
        {
            message = _stringLocalizer["authorizationController.LOGIN.LOGIN_SUCCESFUL"].ToString(),
            redirectUrl = "/Home/Index"
        });
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
