using System.ComponentModel.DataAnnotations;

namespace Dtos.Auth;

public class LoginDto
{
    private string _userName;

    [Required]
    [StringLength(256)]
    public string Username
    {
        get { return _userName; }
        set { _userName = value?.Trim()?.ToLower(); }
    }

    [Required]
    public string Password { get; set; }
}