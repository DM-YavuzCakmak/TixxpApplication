using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Core.Utilities.Attributes;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Bank;

[Table("bank")]
[UseCommonSchema] 
public class BankEntity : BaseEntity
{
    [Column("name")]
    public string Name { get; set; }

    [Column("accountingcode")]
    public string AccountingCode { get; set; }
}
