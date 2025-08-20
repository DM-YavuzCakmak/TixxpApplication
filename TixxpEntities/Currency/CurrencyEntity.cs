using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.CurrencyType;

namespace Tixxp.Entities.Currency;

[Table("Currency")]
public class CurrencyEntity : BaseEntity
{
    [Column("CurrencyId")]
    public long CurrencyId { get; set; }

    [Column("SecondaryCurrencyId")]
    public long SecondaryCurrencyId { get; set; }

    [Column("CurrencyTypeId")]
    public long CurrencyTypeId { get; set; }

    public decimal Value { get; set; }


    [ForeignKey(nameof(CurrencyId))]
    public virtual CurrencyEntity Currency { get; set; }

    [ForeignKey(nameof(SecondaryCurrencyId))]
    public virtual CurrencyEntity SecondaryCurrency { get; set; }

    [ForeignKey(nameof(CurrencyTypeId))]
    public virtual CurrencyTypeEntity CurrencyType { get; set; }
}
