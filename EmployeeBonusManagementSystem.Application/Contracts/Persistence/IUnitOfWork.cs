using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;

namespace EmployeeBonusManagementSystem.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
	    ISqlCommandRepository SqlCommandRepository { get; }
	    ISqlQueryRepository SqlQueryRepository { get; }
	    IBonusRepository BonusRepository { get; }
	    IEmployeeRepository EmployeeRepository { get; }
	    IReportRepository ReportRepository { get; }

		IDbTransaction BeginTransaction(); 
	    Task BeginTransactionAsync(); 
	    void Commit();
	    Task CommitAsync();
	    void Rollback();
	    Task RollbackAsync();
	    IDbConnection Connection { get; } 
	    Task<int> CompleteAsync();
	    Task CloseAsync();
	    Task OpenAsync();
	    IDbTransaction Transaction { get; }


    }
}
