using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Infrastructure.Repositories;
using EmployeeBonusManagementSystem.Persistence.Repositories.Common;
using EmployeeBonusManagementSystem.Persistence.Repositories.Common;

using EmployeeBonusManagementSystem.Persistence.Repositories.Implementations;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Persistence.Repositories;
using Microsoft.Data.SqlClient;

public class UnitOfWork : IUnitOfWork
{
	private readonly IDbConnection _connection;
	private IDbTransaction _transaction;
	private bool _disposed;

	public ISqlCommandRepository SqlCommandRepository { get; }
	public ISqlQueryRepository SqlQueryRepository { get; }
	public IBonusRepository BonusRepository { get; }
	public IEmployeeRepository EmployeeRepository { get; }
	public IReportRepository ReportRepository { get; }
	public ILoggingRepository LoggingRepository { get; }
	public IRefreshTokenRepository RefreshTokenRepository { get; }

	public UnitOfWork(IConfiguration configuration,
					  ISqlCommandRepository sqlCommandRepository,
					  ISqlQueryRepository sqlQueryRepository,
					  IBonusRepository bonusRepository,
					  IEmployeeRepository employeeRepository,
					  IReportRepository reportRepository,
					  ILoggingRepository loggingRepository, 					
					  IRefreshTokenRepository refreshTokenRepository)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		_connection = new SqlConnection(connectionString);
		_connection.Open(); // Open the connection when the UnitOfWork is created

		SqlCommandRepository = sqlCommandRepository ?? throw new ArgumentNullException(nameof(sqlCommandRepository));
		SqlQueryRepository = sqlQueryRepository ?? throw new ArgumentNullException(nameof(sqlQueryRepository));
		BonusRepository = bonusRepository ?? throw new ArgumentNullException(nameof(bonusRepository));
		EmployeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
		ReportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
		LoggingRepository = loggingRepository ?? throw new ArgumentNullException(nameof(loggingRepository));
		RefreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
		SetConnectionAndTransactionForRepositories();
	}

	private void SetConnectionAndTransactionForRepositories()
	{
		if (SqlCommandRepository is SqlCommandRepository sqlCommandRepo)
		{
			sqlCommandRepo.SetConnection(_connection);
			sqlCommandRepo.SetTransaction(_transaction);
		}
		if (SqlQueryRepository is SqlQueryRepository sqlQueryRepo)
		{
			sqlQueryRepo.SetConnection(_connection);
			sqlQueryRepo.SetTransaction(_transaction);
		}
		if (BonusRepository is BonusRepository bonusRepo)
		{
			bonusRepo.SetConnection(_connection);
			bonusRepo.SetTransaction(_transaction);
		}
		if (EmployeeRepository is EmployeeRepository employeeRepo)
		{
			employeeRepo.SetConnection(_connection);
			employeeRepo.SetTransaction(_transaction);
		}
		if (ReportRepository is ReportRepository reportRepo)
		{
			reportRepo.SetConnection(_connection);
			reportRepo.SetTransaction(_transaction);
		}

		if (LoggingRepository is LoggingRepository loggingRepo)
		{
			loggingRepo.SetConnection(_connection);
			loggingRepo.SetTransaction(_transaction);
		}

		if (RefreshTokenRepository is RefreshTokenRepository refreshTokenRepo)
		{
			refreshTokenRepo.SetConnection(_connection);
			refreshTokenRepo.SetTransaction(_transaction);
		}

	}

	public IDbTransaction Transaction => _transaction;

	public IDbTransaction BeginTransaction()
	{
		if (_transaction == null)
		{
			_transaction = _connection.BeginTransaction();
			SetConnectionAndTransactionForRepositories();
		}
		return _transaction;
	}

	public void Commit()
	{
		try
		{
			_transaction?.Commit();
		}
		finally
		{
			_transaction?.Dispose();
			_transaction = null;
		}
	}

	public void Rollback()
	{
		try
		{
			_transaction?.Rollback();
		}
		finally
		{
			_transaction?.Dispose();
			_transaction = null;
		}
	}


	public void Dispose()
	{
		if (!_disposed)
		{
			if (_transaction != null)
			{
				try
				{
					_transaction.Rollback();
				}
				finally
				{
					_transaction.Dispose();
				}
			}
			if (_connection?.State == ConnectionState.Open)
			{
				_connection.Close();
			}
			_connection?.Dispose();
			_disposed = true;
		}
	}

	
}