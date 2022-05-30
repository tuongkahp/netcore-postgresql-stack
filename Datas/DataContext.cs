using Datas.Entities;
using Helpers;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : DbContext
{
    //private readonly string _connectionString;
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<GroupRole> GroupRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserRole>().HasKey(table => new { table.RoleId, table.UserId });
        builder.Entity<GroupUser>().HasKey(table => new { table.GroupId, table.UserId });
        builder.Entity<GroupRole>().HasKey(table => new { table.GroupId, table.RoleId });

        // Add user
        builder.Entity<User>().HasData(new User[] {
            new User 
            {
                UserId = 1, 
                Username = "admin", 
                PasswordHash = PasswordHelper.GenerateHash("admin", "123456"), 
                SecurityStamp = "123456", 
                Email = "", 
                EmailConfirmed = true, 
                PhoneNumber = "" ,
                FullName = "Admin",
                CreatedDate = DateTime.Now,
                Status = Constants.Enums.UserStatus.Active
            }
        });

        // Add role
        builder.Entity<Role>().HasData(new Role[] {
            new Role { RoleId = 1, RoleName = "User.View" },
            new Role { RoleId = 2, RoleName = "User.Add" },
            new Role { RoleId = 3, RoleName = "User.Edit" },
            new Role { RoleId = 4, RoleName = "User.Delete" },
            new Role { RoleId = 5, RoleName = "Group.View" },
            new Role { RoleId = 6, RoleName = "Group.Add" },
            new Role { RoleId = 7, RoleName = "Group.Edit" },
            new Role { RoleId = 8, RoleName = "Group.Delete" },
        });

        // Add group
        builder.Entity<Group>().HasData(new Group[] {
            new Group { GroupId = 1, GroupName = "Admin" , IsActived = true},
        });

        builder.Entity<GroupUser>().HasData(new GroupUser[] {
            new GroupUser { GroupId = 1, UserId = 1 },
        });

        builder.Entity<GroupRole>().HasData(new GroupRole[] {
            new GroupRole { GroupId = 1, RoleId = 1 },
            new GroupRole { GroupId = 1, RoleId = 2 },
            new GroupRole { GroupId = 1, RoleId = 3 },
            new GroupRole { GroupId = 1, RoleId = 4 },
            new GroupRole { GroupId = 1, RoleId = 5 },
            new GroupRole { GroupId = 1, RoleId = 6 },
            new GroupRole { GroupId = 1, RoleId = 7 },
            new GroupRole { GroupId = 1, RoleId = 8 },
        });
    }
}