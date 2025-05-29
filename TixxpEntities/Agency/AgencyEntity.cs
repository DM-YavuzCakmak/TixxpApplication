using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.Agency;

[Table("Agency")]
public class AgencyEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }
}
