using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeSalary;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Application.Mapping
{
	using AutoMapper;
	using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator;

	public class EmployeeProfile : Profile
	{
		public EmployeeProfile()
		{
			CreateMap<EmployeeDto, EmployeeEntity>()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest => dest.PersonalNumber, opt => opt.MapFrom(src => src.PersonalNumber))
				.ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
				.ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
				.ForMember(dest => dest.HireDate, opt => opt.MapFrom(src => src.HireDate))
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
				.ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
				.ForMember(dest => dest.RecommenderEmployeeId,
					opt => opt.MapFrom(src => src.RecommenderEmployeeId ?? 0)) // Handle nullable
				.ForMember(dest => dest.CreateByUserId, opt => opt.MapFrom(src => src.CreateByUserId))
				.ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
				.ForMember(dest => dest.IsPasswordChanged, opt => opt.Ignore())
				.ForMember(dest => dest.PasswordChangeDate, opt => opt.Ignore())
				.ForMember(dest => dest.IsActive, opt => opt.Ignore());


			CreateMap<EmployeeEntity, EmployeeDto>();
			CreateMap<EmployeeEntity, GetEmployeeSalaryDto>();
			CreateMap<EmployeeEntity, GetEmployeeRecommenderDto>();

		}
	}
}
