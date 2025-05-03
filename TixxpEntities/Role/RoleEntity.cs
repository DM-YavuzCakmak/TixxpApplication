using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.Role;

[Table("Role")]
public class RoleEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }
}
