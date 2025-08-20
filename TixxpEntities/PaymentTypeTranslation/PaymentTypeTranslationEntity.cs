using Tixxp.Entities.Base;
using Tixxp.Entities.PaymentType;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tixxp.Entities.PaymentTypeTranslation;

[Table("PaymentTypeTranslation")]
public class PaymentTypeTranslationEntity : BaseEntity
{
    [Column("PaymentTypeId")]
    public long PaymentTypeId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(PaymentTypeId))]
    public virtual PaymentTypeEntity PaymentType { get; set; }
}
