using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EmployeeBonusManagementSystem.Persistence
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "EmployeeBonusManagementSystem.API");

			var configuration = new ConfigurationBuilder()
				.SetBasePath(basePath) 
				.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}