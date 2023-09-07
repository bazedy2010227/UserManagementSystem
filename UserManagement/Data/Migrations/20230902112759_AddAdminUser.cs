using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[UserRoles](UserId,RoleId) SELECT '170faef8-f2f5-4d06-b02e-7ba7b61a19ae',Id FROM[Security].[Roles]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Security].[UserRoles] WHERE UserId = '170faef8-f2f5-4d06-b02e-7ba7b61a19ae'");

        }
    }
}
