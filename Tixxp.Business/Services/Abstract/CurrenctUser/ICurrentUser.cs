using System.Security.Claims;

namespace Tixxp.Business.Services.Abstract.CurrenctUser
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        long? UserId { get; }                  
        string? UserName { get; }
        string? Email { get; }
        string? Culture { get; }               
        IEnumerable<Claim> Claims { get; }
        long GetRequiredUserId();              
    }
}
