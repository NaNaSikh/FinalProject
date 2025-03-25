using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class logs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resonce",
                table: "Logs",
                newName: "Responce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responce",
                table: "Logs",
                newName: "Resonce");
        }
    }
}
