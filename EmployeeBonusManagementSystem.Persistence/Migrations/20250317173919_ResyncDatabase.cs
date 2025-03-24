using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ResyncDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendationLevel",
                table: "RecommenderEmployees");

            migrationBuilder.AddColumn<int>(
                name: "RecommendationLevel",
                table: "Bonuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecommendationLevel",
                table: "Bonuses");

            migrationBuilder.AddColumn<int>(
                name: "RecommendationLevel",
                table: "RecommenderEmployees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
