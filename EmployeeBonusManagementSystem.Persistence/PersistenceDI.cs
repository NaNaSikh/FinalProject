using System.Data;
using EmployeeBonusManagement.Application.Services;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Infrastructure.Repositories;
using EmployeeBonusManagementSystem.Persistence.Factory;
using EmployeeBonusManagementSystem.Persistence.Repositories;
using EmployeeBonusManagementSystem.Persistence.Repositories.Common;
using EmployeeBonusManagementSystem.Persistence.Repositories.Implementations;
using EmployeeBonusManagementSystem.Persistence.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeBonusManagementSystem.Persistence;

public static class PersistenceDI
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IDbConnection>(provider =>
			new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IDbConnectionFactory>(provider =>
			new SqlConnectionFactory(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped<IDbTransaction>(provider =>
		{
			var connection = provider.GetRequiredService<IDbConnection>();
			connection.Open();
			return connection.BeginTransaction();
		});

		services.AddScoped<IEmployeeRepository, EmployeeRepository>();
		services.AddScoped<IBonusRepository, BonusRepository>();
		services.AddScoped<IReportRepository, ReportRepository>();
		services.AddScoped<ISqlQueryRepository, SqlQueryRepository>();
		services.AddScoped<ISqlCommandRepository, SqlCommandRepository>();
		services.AddScoped<ILoggingRepository, LoggingRepository>();

		//services.AddScoped<IHttpContextAccessor>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<IUserContextService, UserContextService>();


	//	services.AddScoped<IRequestHandler<AddEmployeeCommand, bool>, AddEmployeeCommandHandler>();

		return services;
   

	}
}

