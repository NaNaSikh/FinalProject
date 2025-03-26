using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BonusEntityConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualBonusPercent",
                table: "BonusConfigurations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActualBonusPercent",
                table: "BonusConfigurations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
