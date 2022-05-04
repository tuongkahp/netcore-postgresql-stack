using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datas.Entities;

public class UserRole
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
}