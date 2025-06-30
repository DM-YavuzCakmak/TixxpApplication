using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.RoleGroup;

namespace Tixxp.Entities.PersonnelRoleGroup;

[Table("PersonnelRoleGroup")]
public class PersonnelRoleGroupEntity : BaseEntity
{
    public long PersonnelId { get; set; }
    public long RoleGroupId { get; set; }

    [ForeignKey(nameof(PersonnelId))]
    public PersonnelEntity Personnel { get; set; }

    [ForeignKey(nameof(RoleGroupId))]
    public RoleGroupEntity RoleGroup { get; set; }
}
