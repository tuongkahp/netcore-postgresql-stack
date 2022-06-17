namespace Dtos.Users;

public class GetUsersResDto : ResponseBase<GetUsersResDto>
{
    public GetUserDataResDto Data { get; set; }
}

public class GetUserDataResDto
{
    public List<UserDto> Users { get; set; }
    public int Total { get; set; }
}