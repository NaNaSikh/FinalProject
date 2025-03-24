using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus
{
    public record GetEmployeeBonusQuery() : IRequest<List<GetEmployeeBonusDto>>

    {
	    public int UserId { get; set; }

	}
}
