namespace Dtos.Users;

public class GetUsersDetailResDto : ResponseBase<GetUsersDetailResDto>
{
    public UserDto Data { get; set; }
}