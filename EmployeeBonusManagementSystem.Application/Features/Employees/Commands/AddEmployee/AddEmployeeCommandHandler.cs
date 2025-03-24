using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;


namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee
{
	public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, bool>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IJwtService _jwtService;

		public AddEmployeeCommandHandler(
			IEmployeeRepository employeeRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IJwtService jwtService)
		{
			_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_jwtService = jwtService;
		}

		public async Task<bool> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)

		{
			using (var transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					Console.WriteLine($"[INFO] Handling AddEmployeeCommand for Employee: {request.EmployeeDto.FirstName}");

					// Map EmployeeDto to EmployeeEntity
					var employee = _mapper.Map<EmployeeEntity>(request.EmployeeDto);
					Console.WriteLine($"UserName after mapping: {employee.UserName}");

					// Hash the password and generate the refresh token
					var hasher = new PasswordHasher<EmployeeEntity>();
					employee.Password = hasher.HashPassword(null, request.EmployeeDto.Password);
					employee.RefreshToken = _jwtService.GenerateRefreshToken();
					employee.CreateDate = DateTime.UtcNow;
					employee.PasswordChangeDate = DateTime.UtcNow;

					// Add the employee
					await _employeeRepository.AddEmployeeAsync(employee, request.EmployeeDto.Role, transaction);

					// Explicitly commit the transaction
					transaction.Commit();
					Console.WriteLine($"[INFO] Employee saved successfully in database: {request.EmployeeDto.FirstName}");
					return true;
				}
				catch (Exception ex)
				{
					// Rollback on failure
					transaction.Rollback();
					Console.WriteLine($"[ERROR] Error occurred: {ex.Message}");
					return false;
				}
			}

		}
	}
}
