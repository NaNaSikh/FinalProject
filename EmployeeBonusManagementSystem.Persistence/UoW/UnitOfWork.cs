using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private bool _disposed;
    private readonly IServiceProvider _serviceProvider;


    public UnitOfWork(ApplicationDbContext context, IDbConnection connection, IServiceProvider serviceProvider)
    {
        _context = context;
        _connection = connection;
        _connection.Open();
        _serviceProvider = serviceProvider;
    }

    public IDbTransaction Transaction => _transaction;

    public ISqlCommandRepository SqlCommandRepository => _serviceProvider.GetService<ISqlCommandRepository>();
    public ISqlQueryRepository SqlQueryRepository => _serviceProvider.GetService<ISqlQueryRepository>();
    public IBonusRepository BonusRepository => _serviceProvider.GetService<IBonusRepository>();
    public IEmployeeRepository EmployeeRepository => _serviceProvider.GetService<IEmployeeRepository>();
    public IReportRepository ReportRepository => _serviceProvider.GetService<IReportRepository>();

    public IDbConnection Connection => _connection;

    // Begin Transaction (private for internal management)
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

    // Commit Transaction (controlled within UnitOfWork)
    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    // Commit async
    public async Task CommitAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    // Rollback Transaction (controlled within UnitOfWork)
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
            Commit(); // Ensure commit is done within the UnitOfWork
            await CommitAsync();
            return 1;
        }
        catch (Exception)
        {
            Rollback(); // Rollback if something goes wrong
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
