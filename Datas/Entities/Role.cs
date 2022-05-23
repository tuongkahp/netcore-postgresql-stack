using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datas.Entities;

[Table("roles")]
public class Role
{
    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [Required]
    [StringLength(256)]
    [Column("role_name")]
    public string RoleName { get; set; }

    [Required]
    [StringLength(256)]
    [Column("is_default")]
    public bool IsDefault { get; set; }
}