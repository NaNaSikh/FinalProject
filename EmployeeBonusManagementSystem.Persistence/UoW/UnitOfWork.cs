using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmployeeBonusManagementSystem.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly IDbConnectionFactory _connectionFactory;
		private IDbConnection _connection;
		private IDbTransaction _transaction;
		private bool _disposed;
		private IServiceProvider serviceProvider;

		

		public UnitOfWork(ApplicationDbContext context, IDbConnectionFactory connectionFactory, IServiceProvider serviceProvider)
		{
			_context = context;
			_connectionFactory = connectionFactory;
			_connection = _connectionFactory.CreateConnection();
			_connection.Open();
			this.serviceProvider = serviceProvider;
		}

		public ISqlCommandRepository SqlCommandRepository => serviceProvider.GetService<ISqlCommandRepository>();
		public ISqlQueryRepository SqlQueryRepository => serviceProvider.GetService<ISqlQueryRepository>();
		public IBonusRepository BonusRepository => serviceProvider.GetService<IBonusRepository>();
		public IEmployeeRepository EmployeeRepository => serviceProvider.GetService<IEmployeeRepository>();
		public IReportRepository ReportRepository => serviceProvider.GetService<IReportRepository>();

		public IDbConnection Connection => _connection;

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
			await _context.Database.BeginTransactionAsync();
		}

		public void Commit()
		{
			_transaction?.Commit();
			_transaction?.Dispose();
			_transaction = null;
		}

		public async Task CommitAsync()
		{
			await _context.Database.CommitTransactionAsync();
		}

		public void Rollback()
		{
			_transaction?.Rollback();
			_transaction?.Dispose();
			_transaction = null;
		}

		public async Task RollbackAsync()
		{
			await _context.Database.RollbackTransactionAsync();
		}

		public async Task CloseAsync()
		{
			if (await _context.Database.CanConnectAsync())
				await _context.Database.CloseConnectionAsync();
		}

		public async Task OpenAsync()
		{
			if (await _context.Database.CanConnectAsync())
				await _context.Database.OpenConnectionAsync();
		}

		public async Task<int> CompleteAsync()
		{
			try
			{
				Commit();
				await CommitAsync();
				return 1;
			}
			catch (Exception)
			{
				Rollback();
				await RollbackAsync();
				return 0;
			}
			finally
			{
				Dispose();
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_transaction?.Dispose();
				if (_connection?.State == ConnectionState.Open)
				{
					_connection.Close();
				}
				_connection?.Dispose();
				_disposed = true;
			}
		}
	}
}
