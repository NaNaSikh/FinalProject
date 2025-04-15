using EmployeeBonusManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Persistence.Services.Interfaces
{
	public interface IDateService
    {
		(DateTime Start, DateTime End) GetDateRange(TimeRange range);

	}
}
