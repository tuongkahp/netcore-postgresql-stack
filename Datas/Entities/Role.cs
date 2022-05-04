using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datas.Entities;

public class Role
{
    public long UserId { get; set; }
    [Required]
    [StringLength(256)]
    public string UserName { get; set; }
    [Required]
    [StringLength(256)]
    public long Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    [Required]
    public Guid SecurityStamp { get; set; }
    [StringLength(12)]
    public string PhoneNumber { get; set; }
}