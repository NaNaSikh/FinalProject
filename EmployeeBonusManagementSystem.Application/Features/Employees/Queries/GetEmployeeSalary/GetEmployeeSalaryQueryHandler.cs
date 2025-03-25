using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary
{
    internal class GetEmployeeSalaryQueryHandler : IRequestHandler<GetEmployeeSalaryQuery, List<GetEmployeeSalaryDto>>
    {
	    private readonly IEmployeeRepository _employeeRepository;
	    private readonly IMapper _mapper;
		//metadatadan id-is wamosagebad 
		private readonly IUserContextService _userContextService;


		public GetEmployeeSalaryQueryHandler(IUserContextService userContextService, IEmployeeRepository employeeRepository, IMapper mapper)
	    {
		    _userContextService = userContextService;
			_employeeRepository = employeeRepository;
		    _mapper = mapper;
	    }

	    public async Task<List<GetEmployeeSalaryDto>> Handle(GetEmployeeSalaryQuery request, CancellationToken cancellationToken)
	    {
			int userId = _userContextService.GetUserId();
			var salary = await _employeeRepository.GetEmployeeSalary(userId);
		    return _mapper.Map<List<GetEmployeeSalaryDto>>(salary);
	    }
    }
}
