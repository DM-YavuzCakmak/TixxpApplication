using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Core.Utilities.Constants.SchemaConstant;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;

namespace Tixxp.Business.Services.Concrete.PersonnelService;

public class PersonnelService : BaseService<PersonnelEntity>, IPersonnelService
{
    private readonly IPersonnelRepository _personnelRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public PersonnelService(IPersonnelRepository personnelRepository, IHttpContextAccessor httpContextAccessor)
        : base(personnelRepository)
    {
        _personnelRepository = personnelRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IDataResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = _personnelRepository.Get(x => x.Email == loginRequestDto.Email && x.IsActive);
        if (user == null)
            return new ErrorDataResult<LoginResponseDto>("Kullanıcı bulunamadı.");

        if (string.IsNullOrEmpty(user.Salt))
            return new ErrorDataResult<LoginResponseDto>("Kullanıcı salt bilgisi eksik.");

        var saltBytes = Convert.FromBase64String(user.Salt);
        var computedHash = GenerateSha256Hash(saltBytes, loginRequestDto.Password);

        if (user.Password != computedHash)
            return new ErrorDataResult<LoginResponseDto>("Şifre hatalı.");

        // ✅ Claims oluştur
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim("FirstName", user.FirstName ?? string.Empty),
            new Claim("LastName", user.LastName ?? string.Empty),
            new Claim("CompanyIdentifier", user.CompanyIdentifier ?? SchemaConstant.Default)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // ✅ Cookie ile oturum başlat
        await _httpContextAccessor.HttpContext.SignInAsync(
       CookieAuthenticationDefaults.AuthenticationScheme,
       principal,
       new AuthenticationProperties
       {
           IsPersistent = false // ❗ Tarayıcı kapanınca cookie silinir
       });

        var loginResponseDto = new LoginResponseDto
        {
            Success = true,
            Message = "Giriş başarılı."
        };

        return new SuccessDataResult<LoginResponseDto>(loginResponseDto, "Giriş başarılı.");
    }


    #region Test
    public IResult Register(LoginRequestDto loginRequestDto)
    {
        var existing = _personnelRepository.Get(x => x.Email == loginRequestDto.Email && x.IsActive);
        if (existing != null)
            return new ErrorResult("Bu e-posta adresi zaten kayıtlı.");

        // Salt üret
        var saltBytes = GenerateSalt();
        var saltBase64 = Convert.ToBase64String(saltBytes);

        var hash = GenerateSha256Hash(saltBytes, loginRequestDto.Password);

        var personnel = new PersonnelEntity
        {
            FirstName = loginRequestDto.Email,
            LastName = loginRequestDto.Email,
            Email = loginRequestDto.Email,
            UserName = loginRequestDto.Email,
            Password = hash,   // <- Girişte karşılaştırılacak asıl hash burasıdır
            Salt = saltBase64, // <- Giriş sırasında tekrar hashlemek için gerekecek
            IsActive = true,
            IsDeleted = false,
            LoginType = 1,
            CreatedBy = 5,
            Created_Date = DateTime.Now
        };

        _personnelRepository.Add(personnel);

        return new SuccessResult("Kayıt başarılı.");
    }

    private byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    private string GenerateSha256Hash(byte[] saltBytes, string plainPassword)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
        var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }
    #endregion
}
