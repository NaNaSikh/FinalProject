using AutoMapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeBonus
{
    internal class GetEmployeeBonusQueryHandler : IRequestHandler<GetEmployeeBonusQuery, List<GetEmployeeBonusDto>>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public GetEmployeeBonusQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper , IHttpContextAccessor httpContextAccessor)
		{
			_employeeRepository = employeeRepository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<GetEmployeeBonusDto>> Handle(GetEmployeeBonusQuery request, CancellationToken cancellationToken)
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id");

			if (userIdClaim == null)
			{
				throw new UnauthorizedAccessException("User ID not found in token.");
			}

			int userId = int.Parse(userIdClaim.Value);

			var bonuses = await _employeeRepository.GetEmployeeBonus(userId);
			return _mapper.Map<List<GetEmployeeBonusDto>>(bonuses);
		}
	}
}
