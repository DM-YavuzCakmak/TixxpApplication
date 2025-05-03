using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Bank;

[Table("bank")]
public class BankEntity : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }

    [Column("accountingcode")]
    public string AccountingCode { get; set; }
}
