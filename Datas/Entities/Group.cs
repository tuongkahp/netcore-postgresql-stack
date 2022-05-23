using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("groups")]
public class Group
{
    [Key]
    [Required]
    [Column("group_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long GroupId { get; set; }

    [Required]
    [StringLength(256)]
    [Column("group_name")]
    public string GroupName { get; set; }

    [Required]
    [StringLength(256)]
    [Column("is_actived")]
    public bool IsActived { get; set; }
}