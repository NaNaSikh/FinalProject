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
    private readonly IUserContextService _userContextService;

		public GetEmployeeRecommenderQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper, IUserContextService userContextService)
    {
	    _employeeRepository = employeeRepository;
	    _mapper = mapper;
	    _userContextService = userContextService;
    }

    public async Task<List<GetEmployeeRecommenderDto>> Handle(GetEmployeeRecommenderQuery request, CancellationToken cancellationToken)
    {
			int userId = _userContextService.GetUserId();

			var bonuses = await _employeeRepository.GetEmployeeRecommender(userId);
			return _mapper.Map<List<GetEmployeeRecommenderDto>>(bonuses);
    }
    }
}
