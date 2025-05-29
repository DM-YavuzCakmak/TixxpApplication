using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.PriceCategory;

[Table("PriceCategory")]
public class PriceCategoryEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }
}
