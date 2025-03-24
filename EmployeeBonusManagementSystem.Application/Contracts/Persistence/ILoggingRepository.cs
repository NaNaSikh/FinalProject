using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence
{
    public interface ILoggingRepository
    {
	    void LogInformation(string message, params object[] args);
	    void LogError(string message, Exception exception, params object[] args);
	}
}
