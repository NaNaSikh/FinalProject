using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourNamespace.Migrations
{
	public partial class RemoveUserIdIndex : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// SQL Server example:
			migrationBuilder.Sql("DROP INDEX IX_Logs_UserId ON Logs;");
			migrationBuilder.Sql("DROP INDEX IX_ErrorLogs_UserId ON ErrorLogs;");


		}

		
	}
}