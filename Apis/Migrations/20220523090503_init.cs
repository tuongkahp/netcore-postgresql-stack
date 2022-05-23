using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Apis.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "group_roles",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_roles", x => new { x.group_id, x.role_id });
                });

            migrationBuilder.CreateTable(
                name: "group_users",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_users", x => new { x.group_id, x.user_id });
                });

            migrationBuilder.CreateTable(
                name: "groups",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_actived = table.Column<bool>(type: "boolean", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.role_id, x.user_id });
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    full_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    security_stamp = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.InsertData(
                table: "group_roles",
                columns: new[] { "group_id", "role_id" },
                values: new object[,]
                {
                    { 1L, 1 },
                    { 1L, 2 },
                    { 1L, 3 },
                    { 1L, 4 },
                    { 1L, 5 },
                    { 1L, 6 },
                    { 1L, 7 },
                    { 1L, 8 }
                });

            migrationBuilder.InsertData(
                table: "group_users",
                columns: new[] { "group_id", "user_id" },
                values: new object[] { 1L, 1L });

            migrationBuilder.InsertData(
                table: "groups",
                columns: new[] { "group_id", "group_name", "is_actived" },
                values: new object[] { 1L, "Admin", true });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "role_id", "is_default", "role_name" },
                values: new object[,]
                {
                    { 1, false, "User.View" },
                    { 2, false, "User.Add" },
                    { 3, false, "User.Edit" },
                    { 4, false, "User.Delete" },
                    { 5, false, "Group.View" },
                    { 6, false, "Group.Add" },
                    { 7, false, "Group.Edit" },
                    { 8, false, "Group.Delete" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "email", "email_confirmed", "full_name", "password_hash", "phone_number", "security_stamp", "user_name" },
                values: new object[] { 1L, "", true, "Admin", "rA59A3gXCU6eC0RB+brjIJ1nsC+khJFwZfcbFhCaGng=", "", "123456", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_roles");

            migrationBuilder.DropTable(
                name: "group_users");

            migrationBuilder.DropTable(
                name: "groups");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
