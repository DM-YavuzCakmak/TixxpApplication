using Tixxp.Core.Utilities.Results.Abstract;

namespace Tixxp.Business.DataTransferObjects.Personnel.Login;

public class LoginResponseDto : IResult
{
    public bool Success { get; set; }

    public string Message { get; set; }
}
