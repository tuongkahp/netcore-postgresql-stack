using System.ComponentModel.DataAnnotations;

namespace Dtos.Users;

public class LoginDto
{
    private string _userName;

    [Required]
    [StringLength(256)]
    public string UserName
    {
        get { return _userName; }
        set { _userName = value?.Trim()?.ToLower(); }
    }

    [Required]
    public string Password { get; set; }
}