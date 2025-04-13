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
using Microsoft.Extensions.Configuration;


namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee
{
	public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, AddEmploeeResponseDto>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ILoggingRepository _loggingRepository;
		private readonly IUserContextService _userContextService;
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		private readonly IJwtService _jwtService;
		private readonly IConfiguration _config;


		public AddEmployeeCommandHandler(
			IEmployeeRepository employeeRepository,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			ILoggingRepository loggingRepository,
			IUserContextService userContextService,
			IRefreshTokenRepository refreshTokenRepository,
			IJwtService jwtService,
			IConfiguration config)
		{
			_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_loggingRepository = loggingRepository;
			_userContextService = userContextService;
			_refreshTokenRepository = refreshTokenRepository;
			_jwtService = jwtService;
			_config = config;

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
					employee.CreateDate = DateTime.UtcNow;
					employee.PasswordChangeDate = DateTime.UtcNow;
					employee.CreateByUserId = userId;


					await _unitOfWork.EmployeeRepository.AddEmployeeAsync(employee, request.EmployeeDto.Role);

					var newRefreshToken = new RefreshTokenEntity
					{
						EmployeeId = userId,
						ExpirationDate =
							DateTime.UtcNow.AddDays(Convert.ToInt32(_config["RefreshToken:ExpirationDays"])),
						RefreshToken = _jwtService.GenerateRefreshToken()
					};
					await _unitOfWork.RefreshTokenRepository.AddNewRefreshTokenAsync(newRefreshToken);
					var response = new AddEmploeeResponseDto()
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
					await _unitOfWork.LoggingRepository.LogInformationAsync(logEntity);

					 _unitOfWork.Commit(); // Commit the transaction at the handler level
					return response;
				}
				catch (Exception ex)
				{
					 _unitOfWork.Rollback(); // Rollback the transaction on error
					Console.WriteLine($"[ERROR] Error occurred: {ex.Message}");

					var errorLog = new ErrorLogsEntity
					{
						TimeStamp = DateTime.UtcNow,
						UserId = _userContextService.GetUserId(),
						Level = "Error",
						Message = "An error occurred while adding an employee.",
						Exception = ex.ToString()
					};
					await _unitOfWork.LoggingRepository.LogErrorInformationAsync(errorLog);

					return new AddEmploeeResponseDto() { Success = false, Message = "Employee was not added" };
				}
			}
		}

	
	}
}
