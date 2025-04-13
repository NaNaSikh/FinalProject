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
	    
		IDbTransaction BeginTransaction(); 
	    void Commit();
	    void Rollback();
	    ISqlCommandRepository SqlCommandRepository { get; }
	    ISqlQueryRepository SqlQueryRepository { get; }
	    IBonusRepository BonusRepository { get; }
	    IEmployeeRepository EmployeeRepository { get; }
	    IReportRepository ReportRepository { get; }
	    ILoggingRepository LoggingRepository { get; }
	    IRefreshTokenRepository RefreshTokenRepository { get; }
	}
}
