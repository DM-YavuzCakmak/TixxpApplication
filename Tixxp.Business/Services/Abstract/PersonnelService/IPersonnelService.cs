using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Personnel;

namespace Tixxp.Business.Services.Abstract.PersonnelService;
public interface IPersonnelService : IBaseService<PersonnelEntity>
{
    public byte[] GenerateSalt(int size = 16);
    public string GenerateSha256Hash(byte[] saltBytes, string plainPassword);
    Task<IDataResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);

}
