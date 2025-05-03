using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.Role;

namespace Tixxp.Entities.PersonnelRole;

[Table("PersonnelRole")]
public class PersonnelRoleEntity : BaseEntity
{
    [Column("PersonnelId")]
    public long PersonnelId { get; set; }

    [Column("RoleId")]
    public long RoleId { get; set; }

    [ForeignKey(nameof(PersonnelId))]
    public virtual PersonnelEntity Personnel { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual RoleEntity Role { get; set; }
}
