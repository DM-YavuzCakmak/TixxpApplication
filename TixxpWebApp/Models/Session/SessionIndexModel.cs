using Tixxp.Entities.Session;

namespace Tixxp.WebApp.Models.Session;

public class SessionIndexModel
{
    public SessionEntity Session { get; set; } = default!;
    public string? StatusName { get; set; }
    public string? TypeName { get; set; }
}
