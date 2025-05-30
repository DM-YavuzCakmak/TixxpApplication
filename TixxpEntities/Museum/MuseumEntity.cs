using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Museum;

[Table("Museum")]
public class MuseumEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Identifier")]
    public string Identifier { get; set; }
}
