using Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
}