namespace Dtos.Users;

public class GetUsersDetailResDto : ResponseBase<GetUsersDetailResDto>
{
    public UserDataResDto Data { get; set; }
}

public class UserDataResDto
{
    public UserDto User { get; set; }
    public List<RoleDto> Roles { get; set; }
    public GroupDto Group { get; set; }
}