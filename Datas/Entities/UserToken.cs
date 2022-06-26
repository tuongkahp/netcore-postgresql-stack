using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("user_tokens")]
public class UserToken
{
    [Key]
    [Column("user_token_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserTokenId { get; set; }

    [Required]
    [Column("user_id")]
    public long UserId { get; set; }

    [Required]
    [Column("refresh_token")]
    [StringLength(512)]
    public string RefreshToken { get; set; }
}