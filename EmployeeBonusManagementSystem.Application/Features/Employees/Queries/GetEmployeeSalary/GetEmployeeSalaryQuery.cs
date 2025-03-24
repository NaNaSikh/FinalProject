using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary
{
	public record GetEmployeeSalaryQuery() : IRequest<List<GetEmployeeSalaryDto>>
	{
		public int UserId { get; set; }
	}
}
