﻿namespace Application.DTOs.Role;

public class RoleUpdateDTO : RoleBaseDTO
{
    public Guid RoleId { get; set; }
    public List<Guid>? PermissionsId { get; set; }

}
