using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Core.Utilities.Constants.SchemaConstant;

namespace Tixxp.Business.Services.Concrete.CurrentUser
{
    public sealed class HttpCurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        private long? _cachedUserId;

        public HttpCurrentUser(IHttpContextAccessor http)
        {
            _http = http;
        }

        // == Core ==
        private HttpContext? Http => _http.HttpContext;
        private ClaimsPrincipal? Principal => Http?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

        public long? UserId
        {
            get
            {
                if (_cachedUserId.HasValue) return _cachedUserId;

                var p = Principal;
                if (p is null) return null;

                // PersonnelService → ClaimTypes.NameIdentifier = user.Id
                var raw = p.FindFirst(ClaimTypes.NameIdentifier);
                if (long.TryParse(raw.Value, out var id))
                    _cachedUserId = id;

                return _cachedUserId;
            }
        }

        public long GetRequiredUserId()
            => UserId ?? throw new UnauthorizedAccessException("Kullanıcı oturumu bulunamadı (UserId).");

        public string? UserName => Principal?.Identity?.Name;              // PersonnelService → ClaimTypes.Name = "First Last"
        public string? Email => Principal?.FindFirst(ClaimTypes.Email).ToString();

        // Custom claims (PersonnelService.GenerateBaseClaims)
        public string? FirstName => Principal?.FindFirst("FirstName").ToString();
        public string? LastName => Principal?.FindFirst("LastName").ToString();
        public string CompanyIdentifier
            => Principal?.FindFirst("CompanyIdentifier").Value ?? SchemaConstant.Default;

        // Kültür (UI)
        public string? Culture =>
            Http?.Features.Get<IRequestCultureFeature>()?.RequestCulture.UICulture.Name
            ?? CultureInfo.CurrentUICulture.Name;

        public IEnumerable<Claim> Claims => Principal?.Claims ?? Enumerable.Empty<Claim>();

        // Roller
        public IEnumerable<string> Roles
            => Principal?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                ?? Enumerable.Empty<string>();

        public bool IsInRole(string role)
            => !string.IsNullOrWhiteSpace(role) && Principal?.IsInRole(role) == true;
    }
}
