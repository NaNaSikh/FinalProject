using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class logs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responce",
                table: "Logs",
                newName: "Response");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Logs",
                newName: "Responce");
        }
    }
}
