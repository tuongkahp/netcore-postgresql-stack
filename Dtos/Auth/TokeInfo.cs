namespace Dtos.Auth;

public class TokenInfo
{
    public string UserName { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
    public List<string> ListRoles { get; set; }
}