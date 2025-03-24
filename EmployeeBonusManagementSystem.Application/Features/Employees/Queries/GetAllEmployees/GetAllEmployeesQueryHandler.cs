using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetAllEmployees
{
		internal class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
		{
			private readonly IEmployeeRepository _employeeRepository;
			private readonly IMapper _mapper;

			public GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
			{
				_employeeRepository = employeeRepository;
				_mapper = mapper;
			}

			public async Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
			{
				var employees = await _employeeRepository.GetAllEmployeesAsync();
				return _mapper.Map<List<EmployeeDto>>(employees);
			}
		}
}
