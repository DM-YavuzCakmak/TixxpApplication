using Tixxp.Core.Utilities.Results.Concrete;

namespace Tixxp.Business.Services.Abstract.Google;

public interface IAuthenticatorService
{
    public DataResult<byte[]> RegisterAuthenticator(string email);
    public Task<DataResult<bool>> ValidateOtpAsync(string email, string userOtp);
}
