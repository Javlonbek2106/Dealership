using Application.DTOs.Permission;

namespace Application.DTOs.Role;

public class RoleGetDTO : RoleBaseDTO
{
    public Guid Id { get; set; }
    public List<PermissionGetDTO>? Permissions { get; set; }

}
