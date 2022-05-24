using System.ComponentModel.DataAnnotations;

namespace Dtos.Auth;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 6)]
    public string NewPassword { get; set; }
}