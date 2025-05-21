using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tixxp.Core.Utilities.Constants.SchemaConstant;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;

namespace Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;

public class SchemaProvider : ISchemaProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SchemaProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetSchema()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var companyClaim = httpContext?.User?.FindFirst("CompanyIdentifier")?.Value;
        if (!string.IsNullOrWhiteSpace(companyClaim))
        {
            return companyClaim;
        }

        // 2️⃣ Login yoksa, header'dan al (örneğin Postman'den test ederken)
        var fromHeader = httpContext?.Request?.Headers["X-Company-Identifier"].FirstOrDefault();
        return string.IsNullOrWhiteSpace(fromHeader) ? SchemaConstant.Default : fromHeader;
    }

}
