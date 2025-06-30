using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Company;

[Table("Company")]
public class CompanyEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Identifier")]
    public string Identifier { get; set; }
}
