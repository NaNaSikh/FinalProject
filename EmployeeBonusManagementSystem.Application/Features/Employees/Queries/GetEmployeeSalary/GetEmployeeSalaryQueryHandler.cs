using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary
{
    internal class GetEmployeeSalaryQueryHandler : IRequestHandler<GetEmployeeSalaryQuery, List<GetEmployeeSalaryDto>>
    {
		private readonly IUnitOfWork _unitOfWork;
	    private readonly IMapper _mapper;
		private readonly IUserContextService _userContextService;


		public GetEmployeeSalaryQueryHandler(IUnitOfWork unitOfWork,IUserContextService userContextService, IEmployeeRepository employeeRepository, IMapper mapper)
	    {
		    _userContextService = userContextService;
			_unitOfWork = unitOfWork;
		    _mapper = mapper;
	    }

	    public async Task<List<GetEmployeeSalaryDto>> Handle(GetEmployeeSalaryQuery request, CancellationToken cancellationToken)
	    {
			int userId = _userContextService.GetUserId();
			var salary = await _unitOfWork.EmployeeRepository.GetEmployeeSalary(userId);
		    return _mapper.Map<List<GetEmployeeSalaryDto>>(salary);
	    }
    }
}
