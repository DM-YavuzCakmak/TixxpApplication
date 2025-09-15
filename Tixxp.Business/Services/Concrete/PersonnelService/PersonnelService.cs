using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Business.Services.Concrete.PersonnelRoleService;
using Tixxp.Core.Utilities.Constants.SchemaConstant;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;

namespace Tixxp.Business.Services.Concrete.PersonnelService;

public class PersonnelService : BaseService<PersonnelEntity>, IPersonnelService
{
    private readonly IPersonnelRepository _personnelRepository;
    private readonly IPersonnelRoleService _personnelRoleService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PersonnelService(
        IPersonnelRepository personnelRepository,
        IHttpContextAccessor httpContextAccessor,
        IPersonnelRoleService personnelRoleService)
        : base(personnelRepository)
    {
        _personnelRepository = personnelRepository;
        _httpContextAccessor = httpContextAccessor;
        _personnelRoleService = personnelRoleService;
    }

    public async Task<IDataResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = _personnelRepository.Get(x => x.Email == loginRequestDto.Email && x.IsActive);
        if (user == null)
            return new ErrorDataResult<LoginResponseDto>("Kullanıcı bulunamadı.");

        if (string.IsNullOrWhiteSpace(user.Salt))
            return new ErrorDataResult<LoginResponseDto>("Kullanıcı salt bilgisi eksik.");

        var saltBytes = Convert.FromBase64String(user.Salt);
        var computedHash = GenerateSha256Hash(saltBytes, loginRequestDto.Password);

        if (user.Password != computedHash)
            return new ErrorDataResult<LoginResponseDto>("Şifre hatalı.");

        var claims = GenerateBaseClaims(user);

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = false });

        return new SuccessDataResult<LoginResponseDto>(new LoginResponseDto
        {
            Success = true,
            Message = "Giriş başarılı."
        }, "Giriş başarılı.");
    }

    private List<Claim> GenerateBaseClaims(PersonnelEntity user)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        new Claim("ProfilePhotoPath", $"{user.ProfilePhotoPath}"),
        new Claim("FirstName", user.FirstName ?? string.Empty),
        new Claim("LastName", user.LastName ?? string.Empty),
        new Claim("CompanyIdentifier", user.CompanyIdentifier ?? SchemaConstant.Default)
    };

        var personnelRolesResult = _personnelRoleService
           .GetListWithInclude(x => x.PersonnelId == user.Id && !x.IsDeleted, query => query.Role);

        var personnelRoles = personnelRolesResult?.Data?.ToList() ?? new();

        foreach (var role in personnelRoles.Select(x => x.Role).Where(r => r != null))
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        return claims;
    }


    #region Register & Hash

    public IResult Register(LoginRequestDto loginRequestDto)
    {
        var existingUser = _personnelRepository.Get(x => x.Email == loginRequestDto.Email && x.IsActive);
        if (existingUser != null)
            return new ErrorResult("Bu e-posta adresi zaten kayıtlı.");

        var saltBytes = GenerateSalt();
        var saltBase64 = Convert.ToBase64String(saltBytes);
        var hashedPassword = GenerateSha256Hash(saltBytes, loginRequestDto.Password);

        var newPersonnel = new PersonnelEntity
        {
            FirstName = loginRequestDto.Email,
            LastName = loginRequestDto.Email,
            Email = loginRequestDto.Email,
            UserName = loginRequestDto.Email,
            Password = hashedPassword,
            Salt = saltBase64,
            IsActive = true,
            IsDeleted = false,
            LoginType = 1,
            CreatedBy = 5,
            Created_Date = DateTime.Now
        };

        _personnelRepository.Add(newPersonnel);
        return new SuccessResult("Kayıt başarılı.");
    }

    public byte[] GenerateSalt(int size = 16)
    {
        var salt = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    public string GenerateSha256Hash(byte[] saltBytes, string plainPassword)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
        var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }

    #endregion
}
