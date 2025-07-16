using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.Product;

namespace Tixxp.Entities.ProductTranslation;

[Table("ProductTranslation")]
public class ProductTranslationEntity : BaseEntity
{
    [Column("ProductId")]
    public long ProductId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    #region Navigation Properties 

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity Product { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
    #endregion
}
