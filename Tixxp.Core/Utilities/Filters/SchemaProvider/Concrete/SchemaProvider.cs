using Microsoft.AspNetCore.Http;
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
        var schema = _httpContextAccessor.HttpContext?.Request.Headers["X-Company-Identifier"].FirstOrDefault();
        return string.IsNullOrWhiteSpace(schema) ? SchemaConstant.Default : schema;
    }
}
