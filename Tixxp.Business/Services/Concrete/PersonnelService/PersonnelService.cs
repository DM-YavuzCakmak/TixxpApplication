using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using System.Collections.Generic;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Business.DataTransferObjects.Personnel.CreatePersonnel;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Core.Utilities.Enums.RoleEnum;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Tixxp.Business.Services.Concrete.PersonnelService;

public class PersonnelService : BaseService<PersonnelEntity>, IPersonnelService
{
    private readonly IPersonnelRepository _personnelRepository;


    public PersonnelService(IPersonnelRepository personnelRepository)
        : base(personnelRepository)
    {
        _personnelRepository = personnelRepository;
    }

    public IDataResult<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = _personnelRepository.Get(x =>x.Email == loginRequestDto.Email && x.IsActive);
        if (user == null)
            return new ErrorDataResult<LoginResponseDto>("Kullanıcı bulunamadı.");

        var hasher = new Microsoft.AspNet.Identity.PasswordHasher();
        var result = hasher.VerifyHashedPassword(user.Password, loginRequestDto.Password);

        if (result == PasswordVerificationResult.Failed)
            return new ErrorDataResult<LoginResponseDto>("Şifre hatalı.");


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
        };


        return new ErrorDataResult<LoginResponseDto>("Şifre hatalı.");
    }
}

