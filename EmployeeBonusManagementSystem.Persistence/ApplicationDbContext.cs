using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeBonusManagementSystem.Persistence;

public class ApplicationDbContext : DbContext
{

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}
	public DbSet<BonusEntity> Bonuses { get; set; }
    public DbSet<BonusConfigurationEntity> BonusConfigurations { get; set; }
    public DbSet<DepartmentEntity> Departments { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<EmployeeDepartmentEntity> EmployeeDepartments { get; set; }
    public DbSet<RecommenderEmployeeEntity> RecommenderEmployees { get; set; }
    public DbSet<RolesEntity> Roles { get; set; }
    public DbSet<EmployeeRoleEntity> EmployeeRole { get; set; }
	public DbSet<ErrorLogsEntity> ErrorLogs { get; set; }
	public DbSet<LogsEntity> Logs { get; set; }
	public DbSet<RefreshTokenEntity> RefreshToken { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
