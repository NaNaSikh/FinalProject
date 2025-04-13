using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.ChaingePassword
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordDto>
    {
	    private readonly IEmployeeRepository _employeeRepository;
	    private readonly IUserContextService _userContextService;
	    private readonly IAuthService _authService;
	    private readonly ILoggingRepository _loggingRepository;
	    private readonly ILogger<ChangePasswordCommandHandler> _logger;
	    private readonly IUnitOfWork _unitOfWork;


		public ChangePasswordCommandHandler (IUnitOfWork unitOfWork ,  IEmployeeRepository employeeRepository, IUserContextService userContextService , IAuthService authService ,ILoggingRepository loggingRepository, ILogger<ChangePasswordCommandHandler> logger)
		{
			_employeeRepository = employeeRepository;
			_userContextService = userContextService;
			_authService = authService;
			_loggingRepository = loggingRepository;
			_logger = logger;
			_unitOfWork = unitOfWork;

		}

		public async Task<ChangePasswordDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)

		{

			if (_authService.ValidatePassword(request.newPassword, out string message))
			{
				int userId = _userContextService.GetUserId();

				using (var transaction = _unitOfWork.BeginTransaction())
				{

					try
					{

						_logger.LogInformation("User {UserId} initiated a password change request.", userId);

						var result = await _authService.CheckPasswordByIdAsync(userId, request.currentPassword);

						if (result == PasswordVerificationResult.Success)
						{

							var hasher = new PasswordHasher<EmployeeEntity>();

							_logger.LogInformation("User {UserId} successfully verified their current password.",
								userId);

							var newHashedPassword = hasher.HashPassword(null, request.newPassword);

							await _employeeRepository.UpdateEmployeePasswordByIdAsync(userId, newHashedPassword);

							_logger.LogInformation("User {UserId} changed their password successfully.", userId);
							var response = new ChangePasswordDto(true, "Password changed successfully.");


							var logEntity = new LogsEntity
							{
								TimeStamp = DateTime.UtcNow,
								UserId = userId,
								ActionType = "PasswordChange",
								Request = JsonSerializer.Serialize(request),
								Response = JsonSerializer.Serialize(response)

							};
							await _loggingRepository.LogInformationAsync(logEntity);

							_unitOfWork.Commit();

							return response;

						}
						else
						{
							_logger.LogWarning("User {UserId} provided an incorrect current password.", userId);
							return new ChangePasswordDto(false, "Current password is incorrect.");
						}
					}
					catch (Exception ex)
					{ 
						_logger.LogError(ex, "Error updating password for User");
						_unitOfWork.Rollback(); 
						throw;
					}

				}
				
			}
			else
			{
				_logger.LogWarning("User {UserId} provided an invalid new password. Reason: {Message}",
					_userContextService.GetUserId(), message);
				return new ChangePasswordDto(false, $"New password is not valid: {message}");
			}
		}
    }
}
