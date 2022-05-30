using Constants.Enums;

namespace Dtos.Users;

public class UserDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserStatus Status { get; set; }
}