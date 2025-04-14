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

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator
{
    class GetEmployeeRecommenderQueryHandler : IRequestHandler<GetEmployeeRecommenderQuery, List<GetEmployeeRecommenderDto>>
    {
       
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

	public GetEmployeeRecommenderQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
            _unitOfWork = unitOfWork;
	        _mapper = mapper;
	        _userContextService = userContextService;
    }

    public async Task<List<GetEmployeeRecommenderDto>> Handle(GetEmployeeRecommenderQuery request, CancellationToken cancellationToken)
    {
			int userId = _userContextService.GetUserId();

			var bonuses = await _unitOfWork.EmployeeRepository.GetEmployeeRecommender(userId);
			return _mapper.Map<List<GetEmployeeRecommenderDto>>(bonuses);
    }
    }
}
