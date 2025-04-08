using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Application.Mapping
{
	using AutoMapper;
	using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
	using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetAllEmployees;
	using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator;

	public class EmployeeProfile : Profile
	{
		public EmployeeProfile()
		{
			CreateMap<EmployeeDto, EmployeeEntity>()
				.ForMember(dest => dest.RecommenderEmployeeId, opt => opt.MapFrom(src => src.RecommenderEmployeeId ?? 0)) // Handle nullable
				.ForMember(dest => dest.IsPasswordChanged, opt => opt.Ignore())
				.ForMember(dest => dest.PasswordChangeDate, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore());
			CreateMap<GetAllEmployeesDto, EmployeeEntity>();
			CreateMap< EmployeeEntity , GetAllEmployeesDto>();
			CreateMap<EmployeeEntity, EmployeeDto>();
			CreateMap<EmployeeEntity, GetEmployeeSalaryDto>();
			CreateMap<EmployeeEntity, GetEmployeeRecommenderDto>();

		}
	}
}
