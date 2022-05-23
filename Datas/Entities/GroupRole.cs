using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("group_roles")]
public class GroupRole
{
    [Required]
    [Column("group_id")]
    public long GroupId { get; set; }

    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }
}