using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence
{
	public interface ILoggingRepository
	{
		Task LogInformationAsync(LogsEntity logsEntity);
		Task LogErrorInformationAsync(ErrorLogsEntity errorLogsEntity);
	}
}
