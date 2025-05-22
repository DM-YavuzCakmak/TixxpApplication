using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.InvoiceType;

[Table("InvoiceType")]
public class InvoiceTypeEntity : BaseEntity
{
    public string Name { get; set; }
}
