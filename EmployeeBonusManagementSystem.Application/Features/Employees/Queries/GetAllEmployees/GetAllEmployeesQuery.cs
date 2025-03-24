using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries;

public record GetAllEmployeesQuery() : IRequest<List<EmployeeDto>>;
