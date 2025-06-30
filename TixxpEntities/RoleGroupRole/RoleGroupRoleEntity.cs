using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Role;
using Tixxp.Entities.RoleGroup;

namespace Tixxp.Entities.RoleGroupRole;

[Table("RoleGroupRole")]
public class RoleGroupRoleEntity : BaseEntity
{

    [Column("RoleGroupId")]
    public long RoleGroupId { get; set; }

    [Column("RoleId")]
    public long RoleId { get; set; }

    [ForeignKey(nameof(RoleGroupId))]
    public virtual RoleGroupEntity RoleGroup { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual RoleEntity Role { get; set; }
}
