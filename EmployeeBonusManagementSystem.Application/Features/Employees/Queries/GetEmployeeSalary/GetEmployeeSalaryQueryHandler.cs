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
	    private readonly IHttpContextAccessor _httpContextAccessor;


		public GetEmployeeSalaryQueryHandler(IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository, IMapper mapper)
	    {
		    _httpContextAccessor = httpContextAccessor;
			_employeeRepository = employeeRepository;
		    _mapper = mapper;
	    }

	    public async Task<List<GetEmployeeSalaryDto>> Handle(GetEmployeeSalaryQuery request, CancellationToken cancellationToken)
	    {
		    var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id");

			Console.WriteLine($"User id is : {userIdClaim}");
		    if (userIdClaim == null)
		    {
			    throw new UnauthorizedAccessException("User ID not found in token.");
		    }

		    int userId = int.Parse(userIdClaim.Value);

			var salary = await _employeeRepository.GetEmployeeSalary(userId);
		    return _mapper.Map<List<GetEmployeeSalaryDto>>(salary);
	    }
    }
}
