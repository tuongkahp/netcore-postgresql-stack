using System.ComponentModel.DataAnnotations;

namespace Dtos.Users;

public class CreateUserReqDto
{
    private string _userName;

    [Required]
    [StringLength(256)]
    public string Username
    {
        get => _userName;
        set => _userName = value?.Trim()?.ToLower();
    }

    private string _email;

    [EmailAddress]
    [StringLength(256)]
    public string Email
    {
        get => _email;
        set => _email = value?.Trim()?.ToLower();
    }

    [StringLength(256)]
    public string FullName { get; set; }

    [StringLength(12)]
    public string PhoneNumber { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 6)]
    public string Password { get; set; }

    public List<long> GroupIds { get; set; }
    public List<int> RoleIds { get; set; }
}