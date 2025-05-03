using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Personnel;

namespace Tixxp.Business.Services.Abstract.PersonnelService;
public interface IPersonnelService : IBaseService<PersonnelEntity>
{
    IDataResult<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

}
