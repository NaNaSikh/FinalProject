using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private bool _disposed;
    private readonly IServiceProvider _serviceProvider;

	public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		// Initialize connection from EF Core
		_connection = _context.Database.GetDbConnection();
		_connection.Open();
	}

	public IDbTransaction Transaction => _transaction;

	public ISqlCommandRepository SqlCommandRepository => _serviceProvider.GetRequiredService<ISqlCommandRepository>();
	public ISqlQueryRepository SqlQueryRepository => _serviceProvider.GetRequiredService<ISqlQueryRepository>();
	public IBonusRepository BonusRepository => _serviceProvider.GetRequiredService<IBonusRepository>();
	public IEmployeeRepository EmployeeRepository => _serviceProvider.GetRequiredService<IEmployeeRepository>();
	public IReportRepository ReportRepository => _serviceProvider.GetRequiredService<IReportRepository>();

	public IDbConnection Connection => _connection;

	// Begin Transaction
	public IDbTransaction BeginTransaction()
	{
		if (_transaction == null)
		{
			_transaction = _connection.BeginTransaction();
		}

		return _transaction;
	}

	public async Task BeginTransactionAsync()
	{
		if (_transaction == null)
		{
			 await _context.Database.BeginTransactionAsync();
		}
	}

	// Commit Transaction
	public void Commit()
	{
		_transaction?.Commit();
		_transaction?.Dispose();
		_transaction = null;
	}

	public async Task CommitAsync()
	{
		if (_transaction != null)
		{
			await _context.Database.CommitTransactionAsync();
			_transaction.Dispose();
			_transaction = null;
		}
	}

	// Rollback Transaction
	public void Rollback()
	{
		_transaction?.Rollback();
		_transaction?.Dispose();
		_transaction = null;
	}

	public async Task RollbackAsync()
	{
		if (_transaction != null)
		{
			await _context.Database.RollbackTransactionAsync();
			_transaction.Dispose();
			_transaction = null;
		}
	}

	// Open and Close DB Connection
	public async Task OpenAsync()
	{
		if (_connection.State != ConnectionState.Open)
		{
			await _context.Database.OpenConnectionAsync();
		}
	}

	public async Task CloseAsync()
	{
		if (_connection.State != ConnectionState.Closed)
		{
			await _context.Database.CloseConnectionAsync();
		}
	}

	// Save Changes and Commit
	public async Task<int> CompleteAsync()
	{
		try
		{
			int result = await _context.SaveChangesAsync();
			await CommitAsync();
			return result;
		}
		catch (Exception)
		{
			await RollbackAsync();
			throw;
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_transaction?.Dispose();

			if (_connection.State == ConnectionState.Open)
			{
				_connection.Close();
			}

			_connection.Dispose();
			_disposed = true;
		}
	}
}
