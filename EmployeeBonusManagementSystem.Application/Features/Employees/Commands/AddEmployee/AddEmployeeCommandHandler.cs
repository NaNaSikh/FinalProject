using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
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
	public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, AddEmploeeResponseDto>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IJwtService _jwtService;
		private readonly ILoggingRepository _loggingRepository;
		private readonly IUserContextService _userContextService;

		public AddEmployeeCommandHandler(
			IEmployeeRepository employeeRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IJwtService jwtService,
			ILoggingRepository loggingRepository,
			IUserContextService userContextService)
		{
			_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_jwtService = jwtService;
			_loggingRepository = loggingRepository;
			_userContextService = userContextService;
		}

		public async Task<AddEmploeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)

		{
			using (var transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					int userId = _userContextService.GetUserId();
					Console.WriteLine($"[INFO] Handling AddEmployeeCommand for Employee: {request.EmployeeDto.FirstName}");

					var employee = _mapper.Map<EmployeeEntity>(request.EmployeeDto);
					Console.WriteLine($"UserName after mapping: {employee.UserName}");

					var hasher = new PasswordHasher<EmployeeEntity>();
					employee.Password = hasher.HashPassword(null, request.EmployeeDto.Password);
					employee.RefreshToken = _jwtService.GenerateRefreshToken();
					employee.CreateDate = DateTime.UtcNow;
					employee.PasswordChangeDate = DateTime.UtcNow;
					employee.CreateByUserId = userId;

					await _employeeRepository.AddEmployeeAsync(employee, request.EmployeeDto.Role, transaction);

					var response =  new AddEmploeeResponseDto()
					{
						Success = true,
						Message = "Employee added successfully",
						Id = employee.Id
					};

					var logEntity = new LogsEntity
					{
						TimeStamp = DateTime.UtcNow,
						UserId = userId,
						ActionType = "AddEmployee",
						Request = JsonSerializer.Serialize(request),
						Response = JsonSerializer.Serialize(response)

					};
					await _loggingRepository.LogInformationAsync(logEntity);

					transaction.Commit();
				//	Console.WriteLine($"[INFO] Employee saved successfully in database: {request.EmployeeDto.FirstName}");
					return response;
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					Console.WriteLine($"[ERROR] Error occurred: {ex.Message}");
					return new AddEmploeeResponseDto(){Success = false , Message = "Employee was not edded " };
				}
			}

		}
	}
}
