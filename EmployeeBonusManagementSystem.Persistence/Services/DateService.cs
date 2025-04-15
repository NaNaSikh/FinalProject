using EmployeeBonusManagementSystem.Domain.Enums;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Persistence.Services
{
   public  class DateService : IDateService
    {

		public  (DateTime Start, DateTime End) GetDateRange(TimeRange range)
		{
			var end = DateTime.UtcNow;
			return range switch
			{
				TimeRange.OneMonth => (end.AddMonths(-1), end),
				TimeRange.ThreeMonths => (end.AddMonths(-3), end),
				TimeRange.OneYear => (end.AddYears(-1), end),
				TimeRange.AllTime => (DateTime.MinValue, end),
				_ => throw new ArgumentOutOfRangeException(nameof(range), range, null)
			};
		}
	}
}
