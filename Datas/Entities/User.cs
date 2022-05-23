using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserId { get; set; }

    [Required]
    [StringLength(256)]
    [Column("user_name")]
    public string UserName { get; set; }

    [Required]
    [StringLength(256)]
    [Column("full_name")]
    public string FullName { get; set; }

    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }

    [Column("email_confirmed")]
    public bool EmailConfirmed { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("security_stamp")]
    [StringLength(32)]
    public string SecurityStamp { get; set; }

    [StringLength(12)]
    [Column("phone_number")]
    public string PhoneNumber { get; set; }
}