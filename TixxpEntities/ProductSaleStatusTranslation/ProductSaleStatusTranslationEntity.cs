using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.ProductSaleStatus;
namespace Tixxp.Entities.ProductSaleStatusTranslation;


[Table("ProductSaleStatusTranslation")]
public class ProductSaleStatusTranslationEntity : BaseEntity
{
    [Column("ProductSaleStatusId")]
    public long ProductSaleStatusId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(ProductSaleStatusId))]
    public virtual ProductSaleStatusEntity ProductSaleStatus { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}
