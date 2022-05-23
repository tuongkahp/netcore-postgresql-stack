using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("group_users")]
public class GroupUser
{
    [Required]
    [Column("group_id")]
    public long GroupId { get; set; }

    [Required]
    [Column("user_id")]
    public long UserId { get; set; }
}