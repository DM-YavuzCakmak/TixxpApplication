using System.Security.Claims;

namespace Tixxp.Business.Services.Extension
{
    public static class UserExtension
    {
        public static long? GetUserId(this ClaimsPrincipal user)
        {
            var userIdStr = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return long.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }
}
