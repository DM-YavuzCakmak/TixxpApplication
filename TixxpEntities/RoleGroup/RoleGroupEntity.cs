using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.RoleGroup;

[Table("RoleGroup")]
public class RoleGroupEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }
}
