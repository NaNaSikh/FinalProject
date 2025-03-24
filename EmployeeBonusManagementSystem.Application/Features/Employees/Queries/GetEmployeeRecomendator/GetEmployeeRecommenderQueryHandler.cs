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

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator
{
    class GetEmployeeRecommenderQueryHandler : IRequestHandler<GetEmployeeRecommenderQuery, List<GetEmployeeRecommenderDto>>
    {
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetEmployeeRecommenderQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
	    _employeeRepository = employeeRepository;
	    _mapper = mapper;
	    _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<GetEmployeeRecommenderDto>> Handle(GetEmployeeRecommenderQuery request, CancellationToken cancellationToken)
    {
	    var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id");

	    Console.WriteLine($"User id is : {userIdClaim}");
	    if (userIdClaim == null)
	    {
		    throw new UnauthorizedAccessException("User ID not found in token.");
	    }

	    int userId = int.Parse(userIdClaim.Value);

	    var bonuses = await _employeeRepository.GetEmployeeRecomender(userId);
	    return _mapper.Map<List<GetEmployeeRecommenderDto>>(bonuses);
    }
    }
}
