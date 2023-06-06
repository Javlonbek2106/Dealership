namespace Application.DTOs.Role;

public class RoleCreateDTO : RoleBaseDTO
{
    public List<Guid>? PermissionsId { get; set; }
}
