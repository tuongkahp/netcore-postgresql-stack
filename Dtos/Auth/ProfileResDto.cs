namespace Dtos.Auth;

public class ProfileResDto : ResponseBase<ProfileResDto>
{
    public ProfileDataResDto Data { get; set; }
}

public class ProfileDataResDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> ListRoles { get; set; }
}