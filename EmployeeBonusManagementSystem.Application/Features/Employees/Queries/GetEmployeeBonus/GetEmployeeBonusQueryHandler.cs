using AutoMapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EmployeeBonusManagementSystem.Persistence;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus
{
    internal class GetEmployeeBonusQueryHandler : IRequestHandler<GetEmployeeBonusQuery, List<GetEmployeeBonusDto>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUserContextService _userContextService;

		public GetEmployeeBonusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userContextService = userContextService;
		}

		public async Task<List<GetEmployeeBonusDto>> Handle(GetEmployeeBonusQuery request, CancellationToken cancellationToken)
		{
			int userId = _userContextService.GetUserId();
			var bonuses = await _unitOfWork.EmployeeRepository.GetEmployeeBonus(userId);
			return _mapper.Map<List<GetEmployeeBonusDto>>(bonuses);
		}
	}
}
