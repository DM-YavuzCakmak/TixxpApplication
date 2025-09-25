using System.Security.Claims;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;
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

        /// <summary>
        /// Kişiye ait Authenticator uygulaması için secret key bilgisini günceller.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="secretKey"></param>
        public DataResult<PersonnelEntity> UpdateAuthenticatorKey(string email, string secretKey);


        public List<Claim> GenerateBaseClaims(PersonnelEntity user);
    }
}
