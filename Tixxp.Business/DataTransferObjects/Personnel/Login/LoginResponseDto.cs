using Tixxp.Core.Utilities.Results.Abstract;

namespace Tixxp.Business.DataTransferObjects.Personnel.Login;

public class LoginResponseDto : IResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public long LoginTypeId { get; set; }   // ✅ 1=SMS, 2=Google
    public string Email { get; set; }
    public string SecretKey { get; set; }  // ✅ Google Authenticator için
}
