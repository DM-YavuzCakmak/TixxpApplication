using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.CurrencyType;

[Table("CurrencyType")]
public class CurrencyTypeEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Symbol")]
    public string Symbol { get; set; }

    [Column("Tag")]
    public string Tag { get; set; }
}
