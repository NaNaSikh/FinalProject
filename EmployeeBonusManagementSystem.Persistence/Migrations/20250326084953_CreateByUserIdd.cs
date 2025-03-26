using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateByUserIdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogs_Employees_UserId",
                table: "ErrorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Employees_UserId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_UserId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_ErrorLogs_UserId",
                table: "ErrorLogs");

            //migrationBuilder.RenameColumn(
            //    name: "Data",
            //    table: "Logs",
            //    newName: "Response");

            migrationBuilder.AddColumn<string>(
                name: "Request",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CreateByUserId",
                table: "BonusConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Request",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CreateByUserId",
                table: "BonusConfigurations");

            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Logs",
                newName: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLogs_UserId",
                table: "ErrorLogs",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogs_Employees_UserId",
                table: "ErrorLogs",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Employees_UserId",
                table: "Logs",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
