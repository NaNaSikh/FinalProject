﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CreateByUserId",
                table: "BonusConfigurations");
        }
    }
}
