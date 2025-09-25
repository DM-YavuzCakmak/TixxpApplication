using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OtpNet;
using QRCoder;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Google;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Personnel;

namespace Tixxp.Business.Services.Concrete.Google;

/// <summary>
/// Google Authenticator entegrasyonu için TOTP üretim, QR kod oluşturma ve OTP doğrulama servisi.
/// </summary>
public class AuthenticatorService : IAuthenticatorService
{
    private readonly IPersonnelService _personnelService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string Issuer = "TixxpApp";

    public AuthenticatorService(
        IPersonnelService personnelService,
        IHttpContextAccessor httpContextAccessor)
    {
        _personnelService = personnelService ?? throw new ArgumentNullException(nameof(personnelService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <summary>
    /// Kullanıcıya özel bir TOTP secret key üretir ve QR kod döner.
    /// </summary>
    public DataResult<byte[]> RegisterAuthenticator(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new ErrorDataResult<byte[]>("Email geçerli olmalıdır.");

        try
        {
            // 1. Random Secret Key üret
            var secretKeyBytes = KeyGeneration.GenerateRandomKey(20);
            var secretKey = Base32Encoding.ToString(secretKeyBytes);

            // 2. Kullanıcının hesabına secret key'i kaydet
            var updateResult = _personnelService.UpdateAuthenticatorKey(email, secretKey);
            if (!updateResult.Success)
                return new ErrorDataResult<byte[]>($"Secret key DB'ye kaydedilemedi: {updateResult.Message}");

            // 3. TOTP URI oluştur
            var totpUrl = GenerateTotpUri(email, secretKey);

            // 4. QR Kod üret
            var qrCodeBytes = GenerateQrCode(totpUrl);

            return new SuccessDataResult<byte[]>(qrCodeBytes, "Authenticator kaydı başarılı.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<byte[]>($"Authenticator kaydı sırasında hata: {ex.Message}");
        }
    }

    /// <summary>
    /// Kullanıcıdan gelen OTP kodunu doğrular ve başarılıysa cookie oluşturur.
    /// </summary>
    public async Task<DataResult<bool>> ValidateOtpAsync(string email, string userOtp)
    {
        if (string.IsNullOrWhiteSpace(userOtp))
            return new ErrorDataResult<bool>("OTP kodu boş olamaz.");

        try
        {
            // Kullanıcı bilgilerini çek
            var personnelResult = _personnelService.GetFirstOrDefault(x => x.Email == email);
            var personnel = personnelResult?.Data;

            if (personnel == null || string.IsNullOrWhiteSpace(personnel.SecretKey))
                return new ErrorDataResult<bool>("Kullanıcı veya secret key bulunamadı.");

            // OTP doğrulama
            var totp = new Totp(Base32Encoding.ToBytes(personnel.SecretKey));
            var isValid = totp.VerifyTotp(userOtp, out _);

            if (!isValid)
                return new ErrorDataResult<bool>("OTP doğrulama başarısız.");

            // Cookie oluştur
            var claims = _personnelService.GenerateBaseClaims(personnel);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = false });

            return new SuccessDataResult<bool>(true, "OTP doğrulama başarılı.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<bool>($"OTP doğrulama hatası: {ex.Message}");
        }
    }

    private string GenerateTotpUri(string email, string secretKey)
    {
        return $"otpauth://totp/{Issuer}:{email}?secret={secretKey}&issuer={Issuer}";
    }

    private byte[] GenerateQrCode(string totpUri)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
}
