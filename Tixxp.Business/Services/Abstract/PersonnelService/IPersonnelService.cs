using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Personnel;

namespace Tixxp.Business.Services.Abstract.PersonnelService
{
    public interface IPersonnelService : IBaseService<PersonnelEntity>
    {
        /// <summary>
        /// Kullanıcının giriş işlemini gerçekleştirir.
        /// </summary>
        Task<IDataResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Belirtilen boyutta rastgele salt byte dizisi üretir.
        /// </summary>
        byte[] GenerateSalt(int size = 16);

        /// <summary>
        /// Salt ile birleştirilmiş parolayı SHA256 algoritmasıyla hashler.
        /// </summary>
        string GenerateSha256Hash(byte[] saltBytes, string plainPassword);
    }
}
