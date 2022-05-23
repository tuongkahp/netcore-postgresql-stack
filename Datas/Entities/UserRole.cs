﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Datas.Entities;

[Table("user_roles")]
public class UserRole
{
    [Required]
    [Column("user_id")]
    public long UserId { get; set; }

    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }
}