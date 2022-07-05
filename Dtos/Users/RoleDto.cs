using System;
namespace Dtos.Users;

public class RoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsGroupRole { get; set; }
}