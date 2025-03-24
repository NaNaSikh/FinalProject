﻿using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee
{

	public record AddEmployeeCommand : IRequest<bool>
	{
		public EmployeeDto EmployeeDto { get; }

		public AddEmployeeCommand(EmployeeDto employeeDto)
		{
			EmployeeDto = employeeDto;
		}
	}
}
