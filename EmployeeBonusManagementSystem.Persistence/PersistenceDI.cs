using EmployeeBonusManagement.Application.Services;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Infrastructure.Repositories;
using EmployeeBonusManagementSystem.Persistence.Repositories;
using EmployeeBonusManagementSystem.Persistence.Repositories.Common;
using EmployeeBonusManagementSystem.Persistence.Repositories.Implementations;
using EmployeeBonusManagementSystem.Persistence.Services;
using EmployeeBonusManagementSystem.Persistence.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

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
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICheckPasswordService, CheckPasswordService>();
		services.AddScoped<IDateService, DateService>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserContextService, UserContextService>();

        return services;

    }
}

