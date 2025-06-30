namespace Tixxp.WebApp.Models.RoleGroupRole
{
    public class AssignRoleGroupRolesRequest
    {
        public long RoleGroupId { get; set; }
        public List<long> RoleIds { get; set; }
    }
}
